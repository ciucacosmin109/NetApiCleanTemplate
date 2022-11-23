using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetApiCleanTemplate.Core.Entities.DemoEntity;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

namespace NetApiCleanTemplate.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly IConfiguration configuration;
    private readonly IMultitenancyManager multitenancyManager;

    // Db Sets
    public DbSet<DemoEntity> DemoEntities => Set<DemoEntity>();

    // Constructors
    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IConfiguration configuration,
        IMultitenancyManager multitenancyManager
    ) 
        : base(options) 
    {
        this.configuration = configuration;
        this.multitenancyManager = multitenancyManager;
    }

    // Methods
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Get the configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // Multitenancy
        var multitenancyEnabledString = configuration.GetSection("Multitenancy")["Enabled"];
        var multitenancyEnabled = Boolean.Parse(multitenancyEnabledString ?? "False");
        if (multitenancyEnabled)
        {
            // Get the connection string
            var tenant = multitenancyManager.CurrentTenant;
            if (String.IsNullOrWhiteSpace(tenant.DatabaseConnectionString))
            {
                throw new NoDatabaseForTenantException(tenant.TenantId);
            }

            // Replace the configuration from AppDbContextConfigurator
            optionsBuilder.UseSqlServer(tenant.DatabaseConnectionString);
        }
    }
}

