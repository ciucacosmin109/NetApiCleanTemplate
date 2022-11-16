using System.ComponentModel.DataAnnotations;
using NetApiCleanTemplate.WebApi.Models;

namespace NetApiCleanTemplate.WebApi.Controllers.Identity.Models;

public class UserInfo
{
    public static readonly UserInfo Anonymous = new UserInfo();

    public bool IsAuthenticated { get; set; } = false;
    public string NameClaimType { get; set; } = String.Empty;
    public string RoleClaimType { get; set; } = String.Empty;
    public IEnumerable<ClaimValue> Claims { get; set; } = new List<ClaimValue>();
}

