using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetApiCleanTemplate.Core.Interfaces;
using NetApiCleanTemplate.Infrastructure.Data;
using NetApiCleanTemplate.Infrastructure.Identity;

namespace NetApiCleanTemplate.Infrastructure;

public static class Dependencies
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Databases
        if (UseOnlyInMemoryDatabase(configuration))
        {
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("NetApiCleanTemplate.Database")); 
            services.AddDbContext<AppIdentityDbContext>(o => o.UseInMemoryDatabase("NetApiCleanTemplate.Identity"));
        }
        else
        {
            // use real databases
            // Requires LocalDB which can be installed with SQL Server Express 2016
            // https://www.microsoft.com/en-us/download/details.aspx?id=54284

            // Add App DbContext
            services.AddDbContext<AppDbContext>(c =>
                c.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            // Add Identity DbContext
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"))
            );
        }

        // Others
        services.AddMemoryCache();

        // TO DO: Replace with an automatic method
        services.AddTransient<ITokenClaimsService, IdentityTokenClaimService>();
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

