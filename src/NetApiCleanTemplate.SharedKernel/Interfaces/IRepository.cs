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
}

public interface IRepository<TEntity> : IRepository<TEntity, int>
    where TEntity : class, IBaseEntity<int>
{
}