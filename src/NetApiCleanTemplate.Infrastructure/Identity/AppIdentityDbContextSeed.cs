
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Core.Constants;

namespace NetApiCleanTemplate.Infrastructure.Identity;

public class AppIdentityDbContextSeed
{
    public static async Task SeedAsync(AppIdentityDbContext identityDbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (identityDbContext.Database.IsSqlServer())
        {
            identityDbContext.Database.Migrate();
        }

        string userName = "admin@ms.com";
        var defaultUser = new IdentityUser { UserName = userName, Email = userName };
        await userManager.CreateAsync(defaultUser, AuthorizationConstants.DEFAULT_PASSWORD);
        
        string adminUserName = "admin@ms.com";
        var adminUser = new IdentityUser { UserName = adminUserName, Email = adminUserName };
        await userManager.CreateAsync(adminUser,   AuthorizationConstants.DEFAULT_PASSWORD);

        //adminUser = await userManager.FindByNameAsync(adminUserName); 
        //await roleManager.CreateAsync(new IdentityRole(BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS));
        //await userManager.AddToRoleAsync(adminUser, BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS);
    }
}

