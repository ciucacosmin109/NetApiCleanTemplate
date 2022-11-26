using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetApiCleanTemplate.SharedKernel.Interfaces.Multitenancy;
using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Models;

public class RegistrationRequest : BaseRequest
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [DefaultValue(new[] { Tenant.DefaultTenantId })]
    public string[] TenantIds { get; set; } = new[] { Tenant.DefaultTenantId };
}

