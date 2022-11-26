using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetApiCleanTemplate.Infrastructure.Identity.Entities;

public class AppTenant
{
    public int Id { get; set; }

    public string TenantId { get; set; } = "";
    public string? DatabaseConnectionString { get; set; }

    public ICollection<AppUserTenant> AppUserTenants { get; set; } = new List<AppUserTenant>();
}
