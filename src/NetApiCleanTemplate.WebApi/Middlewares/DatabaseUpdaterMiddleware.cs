using System.Net;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Interfaces.Database;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;
using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Middlewares;

public class DatabaseUpdaterMiddleware
{
    private readonly RequestDelegate _next;

    public DatabaseUpdaterMiddleware(RequestDelegate next) // Use this after the multitenancy middleware to have the dbcontext configured with the correct tenant
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext,
        IConfiguration configuration,
        IDatabaseUpdateService updateService
    ) {
        if (Boolean.Parse(configuration["AutoUpdateDatabase"] ?? "False") == true) {
            await updateService.Update();
        }

        await _next(httpContext);
    }

}

