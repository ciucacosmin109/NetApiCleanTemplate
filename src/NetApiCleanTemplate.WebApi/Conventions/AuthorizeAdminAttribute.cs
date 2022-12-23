using Microsoft.AspNetCore.Authorization;

namespace NetApiCleanTemplate.WebApi.Conventions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeAdminAttribute : AuthorizeAttribute
{
    public AuthorizeAdminAttribute() : base(Registration.AdminApiScopePolicy)
    { 
    }
}
