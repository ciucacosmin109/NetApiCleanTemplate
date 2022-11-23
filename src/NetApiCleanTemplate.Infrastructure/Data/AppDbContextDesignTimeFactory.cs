using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Infrastructure.Multitenancy;
using NetApiCleanTemplate.SharedKernel.Configuration;
using Microsoft.Extensions.Configuration;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

namespace NetApiCleanTemplate.Infrastructure.Data;

public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Read the appsettings.json file
        var configuration = AppConfigurations.Get();
        configuration.GetSection("Multitenancy")["Enabled"] = "false";

        // Sa pot actualiza bazele in functie de tenant
        var multitenancyManager = new AppsettingsMultitenancyManager(
            configuration
        );
        var tenantId = GetTenantIdFromCommandLine(args);
        multitenancyManager.SetTenant(tenantId);
        Console.WriteLine($"Tenant: {tenantId}");
        Console.WriteLine($"Connection string: {multitenancyManager.CurrentTenant.DatabaseConnectionString}");

        // DbContext config
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        AppDbContextConfigurator.Configure(optionsBuilder, multitenancyManager.CurrentTenant.DatabaseConnectionString);

        // The IDesignTimeDbContextFactory can not use DI
        return new AppDbContext(
            optionsBuilder.Options,
            configuration,
            multitenancyManager
        );

        // TODO: Sa salvez tenant-ul in functie de user
    }

    private string GetTenantIdFromCommandLine(string[] args)
    {
        var tenantArg = args.FirstOrDefault(x => x.Contains("tenant", StringComparison.OrdinalIgnoreCase));
        if (tenantArg != null)
        {
            var tenantSplit = tenantArg.Split("=");
            if (tenantSplit.Length >= 2)
            {
                return tenantSplit[1];
            }
        }
        return Tenant.DefaultTenantId;
    }
}