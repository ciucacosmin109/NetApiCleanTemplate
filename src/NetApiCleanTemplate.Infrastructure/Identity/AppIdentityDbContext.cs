using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Core.Entities.DemoEntity;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;

namespace NetApiCleanTemplate.Infrastructure.Identity;

public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public DbSet<AppTenant> AppTenants => Set<AppTenant>();
    public DbSet<AppUserTenant> AppUserTenants => Set<AppUserTenant>();

    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }

    // Methods
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Get the configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<AppTenant>().HasIndex(x => x.TenantId).IsUnique();
    }
}
