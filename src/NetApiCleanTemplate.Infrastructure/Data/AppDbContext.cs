using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetApiCleanTemplate.Core.Entities.DemoEntity;

namespace NetApiCleanTemplate.Infrastructure.Data;

public class AppDbContext : DbContext
{
    // Db Sets
    public DbSet<DemoEntity> DemoEntities => Set<DemoEntity>();

    // Constructors
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Methods
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Get the configurations from the current assembly
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

