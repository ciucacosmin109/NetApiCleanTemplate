using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetApiCleanTemplate.Core.Entities.DemoEntity;
using NetApiCleanTemplate.Infrastructure.Data;
using NetApiCleanTemplate.Infrastructure.Data.Repositories;
using NetApiCleanTemplate.Infrastructure.Identity;
using NetApiCleanTemplate.Infrastructure.Uow;
using NetApiCleanTemplate.SharedKernel.Interfaces;
using NetApiCleanTemplate.SharedKernel.Interfaces.Uow;
using NetCore.AutoRegisterDi;

namespace NetApiCleanTemplate.Infrastructure;

public static class Registration
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Databases
        if (UseOnlyInMemoryDatabase(configuration))
        {
            // Use an in-memory database
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("NetApiCleanTemplate.Database"));
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("NetApiCleanTemplate.Identity"));
        }
        else
        {
            // Use real databases
            // Requires LocalDB which can be installed with SQL Server Express
            // https://www.microsoft.com/en-us/download/details.aspx?id=54284

            // Add App DbContext
            services.AddDbContext<AppDbContext>(options => {
                AppDbContextConfigurator.Configure(options, configuration);
            });
            services.AddDbContext<AppIdentityDbContext>(options => {
                AppIdentityDbContextConfigurator.Configure(options, configuration);
            });
        }

        // Generic repositories + Uow
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWorkManager, TransactionalUnitOfWorkManager>();

        // Others
        services.AddMemoryCache();

        // Add DI resolution (auto)
        var validSuffixes = new[] { "Repository", "Service", "Provider", "Queries" };
        var assembly = typeof(Registration).Assembly;
        services.RegisterAssemblyPublicNonGenericClasses(assembly)
            .Where(@class => validSuffixes.Any(suffix => @class.Name.EndsWith(suffix)))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

    }

    private static bool UseOnlyInMemoryDatabase(IConfiguration configuration)
    { 
        if (configuration["UseOnlyInMemoryDatabase"] != null)
        {
            return bool.Parse(configuration["UseOnlyInMemoryDatabase"]);
        }
        return false;
    }

}

