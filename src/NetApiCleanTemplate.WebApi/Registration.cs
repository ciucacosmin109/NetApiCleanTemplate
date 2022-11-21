using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using NetApiCleanTemplate.WebApi.Swagger;
using System.Text;
using NetApiCleanTemplate.Core.Constants;
using NetApiCleanTemplate.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace NetApiCleanTemplate.WebApi;

public static class Registration
{
    public static string SwaggerName = "NetApiCleanTemplate API"; 

    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();

        // Add auth
        services.AddCustomAuthentication();
        
        // Add swagger
        services.AddCustomSwagger();
    }

    private static void AddCustomAuthentication(this IServiceCollection services)
    {
        // 1
        services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

        // 2
        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);
        services.AddAuthentication(config =>
        {
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
    }

    private static void AddCustomSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = SwaggerName, Version = "v1" });
            c.EnableAnnotations(); // https://medium.com/c-sharp-progarmming/configure-annotations-in-swagger-documentation-for-asp-net-core-api-8215596907c7
            c.SchemaFilter<CustomSchemaFilters>();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
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
        });
    }
}

