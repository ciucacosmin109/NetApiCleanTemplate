using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace NetApiCleanTemplate.WebApi.Conventions;

public class AddAuthorizeFiltersControllerConvention : IControllerModelConvention
{
    public static bool NeedsAuthorization(string @namespace) {
        return @namespace.Contains("Controllers.Admin");
    }
    public void Apply(ControllerModel controller)
    {
        var @namespace = controller.ControllerType.Namespace ?? "";
        if(!NeedsAuthorization(@namespace))
        {
            return;
        }

        if (@namespace.Contains("Controllers.Admin"))
        {
            controller.Filters.Add(new AuthorizeFilter(Registration.AdminApiScopePolicy));
        }
    }
}
