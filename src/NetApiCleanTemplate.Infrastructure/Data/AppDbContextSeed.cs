
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Core.Constants;
using NetApiCleanTemplate.Infrastructure.Data;

namespace NetApiCleanTemplate.Infrastructure.Identity;

public class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        // TODO: Take the multitenancy into account
        await Task.FromResult(0);
    }
}

