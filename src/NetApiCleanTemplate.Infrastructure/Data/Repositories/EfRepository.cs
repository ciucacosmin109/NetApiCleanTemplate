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

public class EfRepository<TDbContext, TEntity, TPrimaryKey> 
    : EfReadRepository<TDbContext, TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
    where TDbContext : DbContext
{
    private readonly TDbContext context;

    public EfRepository(
        TDbContext context
    ) 
        : base(context) 
    {
        this.context = context;
    }

    #region Insert

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var entities = context.Set<TEntity>();
        await entities.AddAsync(entity);

        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
    {
        var entities = context.Set<TEntity>();
        await entities.AddAsync(entity);

        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
    {
        return IsTransient(entity)
            ? await InsertAsync(entity)
            : await UpdateAsync(entity);
    }
    public async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
    {
        var result = await InsertOrUpdateAsync(entity);
        return result.Id;
    }

    #endregion

    #region Update

    public async Task<TEntity> UpdateAsync(TEntity entity)
    { 
        if(!IsTransient(entity))
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        return entity;
    }
    public async Task<TEntity?> UpdateAsync(TPrimaryKey id, Action<TEntity> updateAction)
    {
        var entity = await GetAsync(id);
        if(entity != null)
        {
            updateAction(entity);
            await context.SaveChangesAsync();
        }

        return entity;
    }

    #endregion

    #region Delete

    public async Task DeleteAsync(TEntity entity)
    {
        if (!IsTransient(entity))
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
    public async Task DeleteAsync(TPrimaryKey id)
    { 
        var entity = await GetAsync(id);
        if (entity != null)
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    { 
        var all = await GetAllAsync(predicate);
        context.RemoveRange(all);
        await context.SaveChangesAsync();
    }

    #endregion

    private static bool IsTransient(TEntity entity)
    {
        if (EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default(TPrimaryKey)))
        {
            return true;
        }

        // Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
        if (typeof(TPrimaryKey) == typeof(int))
        {
            return Convert.ToInt32(entity.Id) <= 0;
        }

        if (typeof(TPrimaryKey) == typeof(long))
        {
            return Convert.ToInt64(entity.Id) <= 0;
        }

        return false;
    }

}

public class EfRepository<TEntity, TPrimaryKey> 
    : EfRepository<AppDbContext, TEntity, TPrimaryKey>, IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
{
    public EfRepository(AppDbContext context) : base(context) { }
}

public class EfRepository<TEntity> 
    : EfRepository<TEntity, int>, IRepository<TEntity>
    where TEntity : class, IBaseEntity<int>
{
    public EfRepository(AppDbContext context) : base(context) { }
}