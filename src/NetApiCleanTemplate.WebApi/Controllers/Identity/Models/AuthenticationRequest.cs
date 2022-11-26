using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;
using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Models;

public class AuthenticationRequest : BaseRequest
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [DefaultValue(Tenant.DefaultTenantId)]
    public string TenantId { get; set; } = Tenant.DefaultTenantId;
}

