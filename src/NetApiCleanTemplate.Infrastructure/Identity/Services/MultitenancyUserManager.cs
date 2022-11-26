using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;
using NetApiCleanTemplate.SharedKernel.Interfaces.Identity;

namespace NetApiCleanTemplate.Infrastructure.Identity.Services;

public class MultitenancyUserManager : UserManager<AppUser>
{
    private readonly AppIdentityDbContext context;

    public MultitenancyUserManager(
        IUserStore<AppUser> store,
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<AppUser> passwordHasher,
        IEnumerable<IUserValidator<AppUser>> userValidators, 
        IEnumerable<IPasswordValidator<AppUser>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors,
        IServiceProvider services, 
        ILogger<UserManager<AppUser>> logger,
        AppIdentityDbContext context
    ) 
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        this.context = context;
    }

    public async Task<ICollection<string>> GetTenantsAsync(AppUser user)
    {
        var query = context.AppUserTenants
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.Tenant.TenantId);

        return await query.ToListAsync();
    }
}
