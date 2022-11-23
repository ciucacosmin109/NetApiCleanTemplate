using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

namespace NetApiCleanTemplate.Infrastructure.Multitenancy;

public class AppsettingsMultitenancyManager : IMultitenancyManager
{
    // Private members
    private Tenant tenant;

    // Current tenant
    public Tenant CurrentTenant => tenant;
    public event TenantChangedEventHandler? OnTenantChanged;

    // DI
    private readonly IConfiguration configuration;
    public AppsettingsMultitenancyManager(
        IConfiguration configuration
    ) {
        this.configuration = configuration;
        
        tenant = GetDefaultTenant();
    }

    // Tenants
    public Tenant[] GetTenants()
    {
        var tenants = configuration
            .GetSection("Multitenancy")
            .GetSection("ConnectionStrings")
            .GetChildren()
            .Select(x => new Tenant(x.Key, x.Value));

        return tenants.ToArray() ?? Array.Empty<Tenant>();
    }
    public void SetTenant(string tenantId)
    {
        var tenants = GetTenants();
        var tenant = tenants.FirstOrDefault(x => x.TenantId == tenantId);

        if (tenant != null)
        {
            this.tenant = tenant;
        }
        else if(tenantId == Tenant.DefaultTenantId)
        {
            this.tenant = GetDefaultTenant();
        }
        else
        {
            throw new NoTenantException(tenantId);
        }

        OnTenantChanged?.Invoke(this, new TenantChangedEventArgs(this.tenant));
    }

    // Default tenant
    private Tenant GetDefaultTenant()
    {
        var connString = configuration.GetConnectionString("DefaultConnection");
        return new Tenant(Tenant.DefaultTenantId, connString ?? "");

    }
}
