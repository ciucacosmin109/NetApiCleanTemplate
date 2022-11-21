using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetApiCleanTemplate.SharedKernel.Interfaces;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.Infrastructure.Data.Repositories;

public class EfReadRepository<TDbContext, TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
    where TDbContext : DbContext
{
    private readonly TDbContext context;

    public EfReadRepository(
        TDbContext context
    ) {
        this.context = context;
    }

    #region Get

    public async Task<TEntity?> GetAsync(TPrimaryKey id) 
    {
        var entities = context.Set<TEntity>();
        return await entities.FindAsync(id);
    }
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) 
    { 
        var entities = context.Set<TEntity>();
        return await entities.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = context.Set<TEntity>();
        return await entities.ToListAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = context.Set<TEntity>();
        return await entities.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        var entities = context.Set<TEntity>();

        IQueryable<TEntity> query = entities;
        foreach(var propertySelector in propertySelectors) {
            query = query.Include(propertySelector);
        }

        return await query.ToListAsync();
    }

    #endregion

    #region Aggregates

    public async Task<int> CountAsync()
    {
        var entities = context.Set<TEntity>();
        return await entities.CountAsync();
    }
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = context.Set<TEntity>();
        return await entities.CountAsync(predicate);
    }
    public async Task<long> LongCountAsync()
    {
        var entities = context.Set<TEntity>();
        return await entities.LongCountAsync();
    }
    public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = context.Set<TEntity>();
        return await entities.LongCountAsync(predicate);
    }

    #endregion

}

public class EfReadRepository<TEntity, TPrimaryKey> 
    : EfRepository<AppDbContext, TEntity, TPrimaryKey>, IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
{
    public EfReadRepository(AppDbContext context) : base(context) { }
}

public class EfReadRepository<TEntity> 
    : EfReadRepository<TEntity, int>, IRepository<TEntity>
    where TEntity : class, IBaseEntity<int>
{
    public EfReadRepository(AppDbContext context) : base(context) { }
}