using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

public class NoDatabaseForTenantException : Exception {
    public NoDatabaseForTenantException(string tenant)
        : base($"There is no database for tenant [{tenant}]")
    {

    }
}
