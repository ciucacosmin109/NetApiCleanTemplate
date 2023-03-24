using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;

namespace NetApiCleanTemplate.SharedKernel.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool HasRight(this ClaimsPrincipal user, string right)
    {
        return user.Claims.Any(x => x.Type == CustomClaimTypes.Right && x.Value == right);
    }

    public static string GetSelectedTenant(this ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(x => x.Type == CustomClaimTypes.SelectedTenant)?.Value ?? Tenant.DefaultTenantId;
    }
    public static bool IsDefaultTenant(this ClaimsPrincipal user)
    {
        return user.GetSelectedTenant() == Tenant.DefaultTenantId;
    }
}
