using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.SharedKernel.Interfaces;

public interface IReadRepository { }

public interface IReadRepository<TEntity, TPrimaryKey> : IReadRepository
    where TEntity : class, IBaseEntity<TPrimaryKey>
{
    #region Get

    Task<TEntity?> GetAsync(TPrimaryKey id);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors);

    #endregion

    #region Aggregates

    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    Task<long> LongCountAsync();
    Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion
}

public interface IReadRepository<TEntity> : IReadRepository<TEntity, int>
    where TEntity : class, IBaseEntity<int>
{
}
