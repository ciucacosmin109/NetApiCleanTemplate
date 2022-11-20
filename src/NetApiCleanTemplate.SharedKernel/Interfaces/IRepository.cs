using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.SharedKernel.Interfaces;

public interface IRepository { }

public interface IRepository<TEntity, TPrimaryKey> : IRepository 
    where TEntity : class, IBaseEntity<TPrimaryKey>
{
    #region Get

    Task<TEntity?> Get(TPrimaryKey id);
    Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAll();
    Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

    #endregion

    #region Insert

    Task<TEntity> Insert(TEntity entity);
    Task<TPrimaryKey> InsertAndGetId(TEntity entity);

    Task<TEntity> InsertOrUpdate(TEntity entity);
    Task<TPrimaryKey> InsertOrUpdateAndGetId(TEntity entity);

    #endregion

    #region Update

    Task<TEntity> Update(TEntity entity);
    Task<TEntity?> Update(TPrimaryKey id, Action<TEntity> updateAction);

    #endregion

    #region Delete

    Task Delete(TEntity entity);
    Task Delete(TPrimaryKey id);
    Task Delete(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #region Aggregates

    Task<int> Count();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<long> LongCountAsync();
    Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion
}

public interface IRepository<TEntity> : IRepository<TEntity, int>
    where TEntity : class, IBaseEntity<int>
{
}