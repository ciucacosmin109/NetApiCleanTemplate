using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.SharedKernel.Interfaces;

public interface IRepository : IReadRepository { }

public interface IRepository<TEntity, TPrimaryKey>
    : IRepository, IReadRepository<TEntity, TPrimaryKey>
    where TEntity : class, IBaseEntity<TPrimaryKey>
{
    #region Insert

    Task<TEntity> InsertAsync(TEntity entity);
    Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

    Task<TEntity> InsertOrUpdateAsync(TEntity entity);
    Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

    #endregion

    #region Update

    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity?> UpdateAsync(TPrimaryKey id, Action<TEntity> updateAction);

    #endregion

    #region Delete

    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(TPrimaryKey id);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion

}

public interface IRepository<TEntity> : IRepository<TEntity, int>
    where TEntity : class, IBaseEntity<int>
{
}