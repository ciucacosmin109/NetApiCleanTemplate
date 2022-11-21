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

public class Repository<TDbContext, TEntity, TPrimaryKey> 
    : IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
    where TDbContext : DbContext
{
    private readonly TDbContext context;

    public Repository(
        TDbContext context
    ) {
        this.context = context;
    }

    #region Get

    public async Task<TEntity?> Get(TPrimaryKey id) 
    {
        var entities = context.Set<TEntity>();
        return await entities.FindAsync(id);
    }
    public async Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate) 
    { 
        var entities = context.Set<TEntity>();
        return await entities.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        var entities = context.Set<TEntity>();
        return await entities.ToListAsync();
    }
    public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = context.Set<TEntity>();
        return await entities.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        var entities = context.Set<TEntity>();

        IQueryable<TEntity> query = entities;
        foreach(var propertySelector in propertySelectors) {
            query = query.Include(propertySelector);
        }

        return await query.ToListAsync();
    }

    #endregion

    #region Insert

    public async Task<TEntity> Insert(TEntity entity)
    {
        var entities = context.Set<TEntity>();
        await entities.AddAsync(entity);

        await context.SaveChangesAsync();
        return entity;
    }
    public async Task<TPrimaryKey> InsertAndGetId(TEntity entity)
    {
        var entities = context.Set<TEntity>();
        await entities.AddAsync(entity);

        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<TEntity> InsertOrUpdate(TEntity entity)
    {
        return IsTransient(entity)
            ? await Insert(entity)
            : await Update(entity);
    }
    public async Task<TPrimaryKey> InsertOrUpdateAndGetId(TEntity entity)
    {
        var result = await InsertOrUpdate(entity);
        return result.Id;
    }

    #endregion

    #region Update

    public async Task<TEntity> Update(TEntity entity)
    { 
        if(!IsTransient(entity))
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        return entity;
    }
    public async Task<TEntity?> Update(TPrimaryKey id, Action<TEntity> updateAction)
    {
        var entity = await Get(id);
        if(entity != null)
        {
            updateAction(entity);
            await context.SaveChangesAsync();
        }

        return entity;
    }

    #endregion

    #region Delete

    public async Task Delete(TEntity entity)
    {
        if (!IsTransient(entity))
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
    public async Task Delete(TPrimaryKey id)
    { 
        var entity = await Get(id);
        if (entity != null)
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }
    }
    public async Task Delete(Expression<Func<TEntity, bool>> predicate)
    { 
        var all = await GetAll(predicate);
        context.RemoveRange(all);
        await context.SaveChangesAsync();
    }

    #endregion

    #region Aggregates

    public async Task<int> Count()
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

public class Repository<TEntity, TPrimaryKey> 
    : Repository<AppDbContext, TEntity, TPrimaryKey>, IRepository<TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
{
    public Repository(AppDbContext context) : base(context) { }
}

public class Repository<TEntity> 
    : Repository<TEntity, int>, IRepository<TEntity>
    where TEntity : class, IBaseEntity<int>
{
    public Repository(AppDbContext context) : base(context) { }
}