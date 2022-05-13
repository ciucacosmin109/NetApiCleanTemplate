using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaCleanTemplate.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaCleanTemplate.Infrastructure;

public class InfrastructureModule : Module
{
    public InfrastructureModule(ICollection<Module> dependencies) : base(dependencies) { }

    public override void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Configure the dependent modules
        base.ConfigureServices(configuration, services);

        // Entity framework
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Authentication 
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        // Exception page for developers 
        services.AddDatabaseDeveloperPageExceptionFilter();

        // Dependency Injection
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }
    public override void Configure(IConfiguration configuration, IApplicationBuilder app, bool isDevEnv)
    {
        // Configure the dependent modules
        base.Configure(configuration, app, isDevEnv);

        // Others
        app.UseMigrationsEndPoint();
    }
}

