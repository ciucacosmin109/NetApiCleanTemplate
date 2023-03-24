using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NetApiCleanTemplate.Infrastructure.Identity;
using NetApiCleanTemplate.SharedKernel.Extensions;

namespace NetApiCleanTemplate.WebApi.Conventions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeRightAttribute : TypeFilterAttribute
{
    public AuthorizeRightAttribute(string right)
        : base(typeof(AuthorizeRightFilter))
    {
        Arguments = new object[] { right };
    }

}
public class AuthorizeRightFilter : IAuthorizationFilter
{
    private readonly string right;
    private readonly AppIdentityDbContext context;

    public AuthorizeRightFilter(
        string right,
        AppIdentityDbContext context
    )
    {
        this.right = right;
        this.context = context;
    }

    public void OnAuthorization(AuthorizationFilterContext aCtx)
    {
        var user = aCtx.HttpContext.User;
        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            // it isn't needed to set unauthorized result 
            // as the base class already requires the user to be authenticated
            // this also makes redirect to a login page work properly
            // context.Result = new UnauthorizedResult();
            return;
        }

        if (!user.HasRight(right))
        {
            //context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            aCtx.Result = new ForbidResult();
            return;
        }

        // Portal
        //var dbRight = context.Rights.FirstOrDefault(x => x.Name == right);
        //if (dbRight.OnlyOnDefaultTenant && !user.IsDefaultTenant())
        //{
        //    //context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        //    aCtx.Result = new ForbidResult();
        //    return;
        //}

        // you can also use registered services
        //var someService = context.HttpContext.RequestServices.GetService<ISomeService>();
        //var isAuthorized = someService.IsUserAuthorized(user.Identity.Name, _someFilterParameter);
        //if (!isAuthorized)
        //{
        //    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        //    return;
        //}
    }
}
