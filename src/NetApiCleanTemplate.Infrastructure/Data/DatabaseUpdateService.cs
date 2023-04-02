using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.SharedKernel.Interfaces.Database;

namespace NetApiCleanTemplate.Infrastructure.Data;

public class DatabaseUpdateService : IDatabaseUpdateService
{
    private readonly AppDbContext context;

    public DatabaseUpdateService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task Update()
    {
        await context.Database.MigrateAsync();
    }
}
