using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

public interface IMultitenancyManager
{
    Tenant CurrentTenant { get; }

    Tenant[] GetTenants();
    void SetTenant(string tenantId);

    event TenantChangedEventHandler OnTenantChanged;
}
