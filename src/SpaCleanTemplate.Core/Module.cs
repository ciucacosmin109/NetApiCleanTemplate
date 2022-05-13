using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaCleanTemplate.Core;

public abstract class Module
{
    private readonly ICollection<Module> _dependencies;

    public Module()
    {
        _dependencies = new List<Module>();
    }
    public Module(ICollection<Module> dependencies)
    {
        if(dependencies != null)
        {
            _dependencies = dependencies;
        }
        else
        {
            _dependencies = new List<Module>();
        }
    }

    public virtual void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        foreach (var dependency in _dependencies)
        {
            dependency.ConfigureServices(configuration, services);
        }
    }
    public virtual void Configure(IConfiguration configuration, IApplicationBuilder app, bool isDevEnv)
    {
        foreach (var dependency in _dependencies)
        {
            dependency.Configure(configuration, app, isDevEnv);
        }
    }
}

