using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using NetApiCleanTemplate.Core.Entities.DemoEntity; 
using NetApiCleanTemplate.SharedKernel.Interfaces;
using NetApiCleanTemplate.SharedKernel.Interfaces.Uow;
using NetCore.AutoRegisterDi;

namespace NetApiCleanTemplate.Core;

public static class Registration
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        // Add DI resolution (auto)
        var assembly = typeof(Registration).Assembly;
        var validSuffixes = new[] { "Service", "Factory", "Commands" };
        services.RegisterAssemblyPublicNonGenericClasses(assembly)
            .Where(@class => validSuffixes.Any(suffix => @class.Name.EndsWith(suffix)))
            .AsPublicImplementedInterfaces(); // The scope is transient by default
    }
}

