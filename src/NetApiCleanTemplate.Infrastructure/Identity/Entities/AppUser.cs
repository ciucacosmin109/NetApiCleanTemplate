using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NetApiCleanTemplate.Infrastructure.Identity.Entities;

public class AppUser : IdentityUser<int>
{
    public bool HasAccessToAllTenants { get; set; }
    public ICollection<AppUserTenant> AppUserTenants { get; set; } = new List<AppUserTenant>();
}
