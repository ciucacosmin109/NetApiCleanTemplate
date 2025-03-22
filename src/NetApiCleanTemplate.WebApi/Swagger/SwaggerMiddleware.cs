using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace NetApiCleanTemplate.WebApi.Swagger;

public static class SwaggerMiddleware
{
    private static readonly string ApiName = "NetApiCleanTemplate API";

    public static void UseSwaggerMiddleware(this IApplicationBuilder app, IConfiguration config)
    {
        var enable = config.GetValue<bool?>("Swagger:Enabled") ?? false;
        if (!enable)
        {
            return;
        }

        app.UseSwagger(options =>
        {
            options.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                // Clear servers -element in swagger.json because it got the wrong port when hosted behind reverse proxy
                swagger.Servers.Clear();
            });
        });
        app.UseSwaggerUI(c => {
            // Core
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Registration.SwaggerName} v1");

            // Portal
            c.OAuthClientId(Registration.SwaggerClientId);
            c.OAuthAppName(Registration.SwaggerName);
            c.OAuthUsePkce();

            // Display
            c.DefaultModelExpandDepth(0);
            c.DefaultModelRendering(ModelRendering.Model);
            c.DefaultModelsExpandDepth(-1);
            c.DocExpansion(DocExpansion.List);

            c.DisplayOperationId();
            c.DisplayRequestDuration();
            c.EnableFilter();
            c.ShowExtensions();

            // Other
            c.DocumentTitle = "NetApiCleanTemplate";
            //c.InjectJavascript("/swagger/multitenancy-auth.js");
        });
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, string? authUrl)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();

        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = ApiName, Version = "v1" });
            c.OperationFilter<AuthorizeCheckOperationFilter>();

            // Req
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            },
                            UnresolvedReference = true
                        },
                        new List<string>()
                    }
                });

            // Portal
            if (authUrl != null)
            {
                c.AddSecurityDefinition("Portal", new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows {
                        AuthorizationCode = new OpenApiOAuthFlow {
                            AuthorizationUrl = new Uri(new Uri(authUrl), "/connect/authorize"),
                            TokenUrl = new Uri(new Uri(authUrl), "/connect/token"),
                            Scopes = new Dictionary<string, string>
                        {
                                {"csrd-backend", $"{ApiName} - full access"},
                                // {"app_extra", "UserInfo: LanguageCode, ProfileId, clientIdentifier, UserName" },
                                {"profile", "Profile scope" },
                                {"openid", "OpenID scope"}
                            }
                        }
                    }
                });
            }

            // Token
            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
                Description = "JWT containing user claims",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            // Api Secret
            c.AddSecurityDefinition("Secret", new OpenApiSecurityScheme() {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "secret",
            });
        });
    }
}