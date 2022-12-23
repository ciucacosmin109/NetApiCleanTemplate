using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using NetApiCleanTemplate.WebApi.Swagger;
using System.Text;
using NetApiCleanTemplate.Core.Constants;
using NetApiCleanTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using NetApiCleanTemplate.WebApi.Conventions;

namespace NetApiCleanTemplate.WebApi;

public static class Registration
{
    public static string AppApiScopeId = "NetApiCleanTemplate_Api";
    public static string AppApiScopeName = "Api";
    public static string AppApiScopePolicy = "ApiScope";

    public static string AdminApiScopeId = "NetApiCleanTemplate_AdminApi";
    public static string AdminApiScopeName = "Admin";
    public static string AdminApiScopePolicy = "AdminScope";

    public static string SwaggerClientId = "NetApiCleanTemplate_Swagger"; 
    public static string SwaggerName = "NetApiCleanTemplate API"; 

    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers(options => {
            options.Conventions.Add(new AddAuthorizeFiltersControllerConvention());
        });

        // Add auth
        //services.AddCustomAuthentication();
        services.AddPortalAuthentication();
        
        // Add swagger
        services.AddCustomSwagger();
    }

    private static void AddCustomAuthentication(this IServiceCollection services)
    {
        // 1
        services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

        // 2
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
        services.AddAuthentication(config => {
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(config => {
            config.RequireHttpsMetadata = false;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        // 3
        services.AddAuthorization(options => {
            // Authenticated
            options.AddPolicy("default", policy => {
                policy.RequireAuthenticatedUser();
            });
            // ApiScope
            options.AddPolicy(AppApiScopePolicy, policy => {
                policy.RequireAuthenticatedUser();
                // TODO
                //policy.RequireClaim("scope", AppApiScopeId);
            });
            // AdminScope
            options.AddPolicy(AdminApiScopePolicy, policy => {
                policy.RequireAuthenticatedUser();
                // TODO
                //policy.RequireClaim("scope", AdminApiScopeId);
            });

            // Configure the default policy
            options.DefaultPolicy = options.GetPolicy("default") ?? throw new ArgumentNullException("options.DefaultPolicy");
        });
    }

    private static void AddPortalAuthentication(this IServiceCollection services)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        // 1
        services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

        // 2
        services.AddAuthentication(config => {
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, config => {
            config.Authority = "https://localhost:44390";

            //config.Audience = Configuration["auth:oidc:clientid"];
            config.TokenValidationParameters = new TokenValidationParameters {
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        // 3
        services.AddAuthorization(options => {
            // Authenticated
            options.AddPolicy("default", policy => {
                policy.RequireAuthenticatedUser();
            });
            // ApiScope
            options.AddPolicy(AppApiScopePolicy, policy => {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", AppApiScopeId);
            });
            // AdminScope
            options.AddPolicy(AdminApiScopePolicy, policy => 
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", AdminApiScopeId);
            });

            // Configure the default policy
            options.DefaultPolicy = options.GetPolicy("default") ?? throw new ArgumentNullException("options.DefaultPolicy");
        });
    }

    private static void AddCustomSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = SwaggerName, Version = "v1" });
            c.EnableAnnotations(); // https://medium.com/c-sharp-progarmming/configure-annotations-in-swagger-documentation-for-asp-net-core-api-8215596907c7
            c.SchemaFilter<CustomSchemaFilters>();
            c.OperationFilter<AuthorizeCheckOperationFilter>();
            
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Description = @"JWT Authorization header using the Bearer scheme. <br />
                    Enter 'Bearer' [space] and then your token in the text input below. <br />
                    Example: 'Bearer 12345abcdef'. <br />",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows {
                    AuthorizationCode = new OpenApiOAuthFlow {
                        AuthorizationUrl = new Uri("https://localhost:44390/connect/authorize"),
                        TokenUrl = new Uri("https://localhost:44390/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {AppApiScopeId, AppApiScopeName},
                            {AdminApiScopeId, AdminApiScopeName}
                        }
                    }
                }
            });
        });
    }

}

