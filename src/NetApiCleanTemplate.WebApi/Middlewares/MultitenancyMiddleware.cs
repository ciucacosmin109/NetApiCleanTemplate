using System.Net;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;
using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Middlewares;

public class MultitenancyMiddleware
{
    private readonly RequestDelegate _next;

    public MultitenancyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        IMultitenancyManager multitenancyManager
    ) {
        var claims = httpContext.User.Claims;
        var tenantClaim = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.SelectedTenant);

        if (tenantClaim != null) {
            var tenant = tenantClaim.Value;
            multitenancyManager.SetTenant(tenant); // IMultitenancyManager must be registered as scoped
        }

        await _next(httpContext);
    }

}

