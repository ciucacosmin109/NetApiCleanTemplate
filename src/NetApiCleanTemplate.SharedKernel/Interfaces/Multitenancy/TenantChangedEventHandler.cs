using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

public class TenantChangedEventArgs : EventArgs
{
    public Tenant NewTenant { get; private set; }

    public TenantChangedEventArgs(Tenant newTenant)
    {
        NewTenant = newTenant;
    }
};

public delegate void TenantChangedEventHandler(object? sender, EventArgs e);
