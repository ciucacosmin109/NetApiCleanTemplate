using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Infrastructure.Identity.Entities;

public class AppUserTenant
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public AppUser User { get; set; } = new();

    public int TenantId { get; set; }
    public AppTenant Tenant { get; set; } = new();
}
