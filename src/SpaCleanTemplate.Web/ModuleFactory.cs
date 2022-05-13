using SpaCleanTemplate.Core;
using SpaCleanTemplate.Infrastructure;
using System.Globalization;

namespace SpaCleanTemplate.Web;

public static class ModuleFactory
{
    public static Module Create(Type type)
    {
        return Create(type, new List<Module>());
    }

    public static Module Create(Type type, Module dependency)
    {
        if (dependency == null)
        {
            return Create(type);
        }

        return Create(type, new List<Module> { dependency });
    }

    public static Module Create(Type type, ICollection<Module> dependencies)
    {
        if (dependencies == null)
        {
            dependencies = new List<Module>();
        }

        switch (type.ToString())
        {
            case "SpaCleanTemplate.Core.CoreModule":
                return new CoreModule();

            case "SpaCleanTemplate.Infrastructure.InfrastructureModule":
                return new InfrastructureModule(dependencies);

            case "SpaCleanTemplate.Web.WebModule":
                return new WebModule(dependencies);

            default:
                throw new ArgumentException($"Unrecognized type [{type.FullName}]", "type");
        }
    }
}

