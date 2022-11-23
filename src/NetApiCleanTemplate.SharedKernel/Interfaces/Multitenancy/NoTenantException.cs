using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

public class NoTenantException : Exception {
    public NoTenantException(string tenant)
        : base($"There is no tenant with the name [{tenant}]")
    {

    }
}
