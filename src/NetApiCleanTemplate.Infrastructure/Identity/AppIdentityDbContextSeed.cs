
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Core.Constants;
using NetApiCleanTemplate.Infrastructure.Identity.Entities;

namespace NetApiCleanTemplate.Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        if (identityDbContext.Database.IsSqlServer())
        {
            identityDbContext.Database.Migrate();
        }

        var defaultUser = new AppUser { 
            UserName = "user", 
            Email = "user@demo.com", 
            AppUserTenants = new List<AppUserTenant> { 
                new() {
                    Tenant = new() {
                        TenantId = "TenantA"
                    }
                },
                new() {
                    Tenant = new() {
                        TenantId = "TenantB"
                    }
                },
            }
        };
        await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);
        
        var adminUser = new AppUser { 
            UserName = "admin", 
            Email = "admin@demo.com", 
            HasAccessToAllTenants = true 
        };
        await userManager.CreateAsync(adminUser,   AuthorizationConstants.DEFAULT_PASSWORD);

        //adminUser = await userManager.FindByNameAsync(adminUserName); 
        //await roleManager.CreateAsync(new AppRole(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS));
        //await userManager.AddToRoleAsync(adminUser, BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
    }
}

