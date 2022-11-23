using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

public class Tenant
{
    public static string DefaultTenantId = String.Empty;

    public string TenantId { get; set; }
    public string DatabaseConnectionString { get; set; }

    public Tenant(string tenantId, string databaseConnectionString)
    {
        TenantId = tenantId;
        DatabaseConnectionString = databaseConnectionString;
    }

    public Tenant()
    {
        TenantId = DefaultTenantId;
        DatabaseConnectionString = String.Empty;
    }
}
