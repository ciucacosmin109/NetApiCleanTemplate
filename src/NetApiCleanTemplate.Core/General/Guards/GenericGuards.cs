using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Constants;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Exceptions;
using NetApiCleanTemplate.Core.General.Exceptions;
using NetApiCleanTemplate.SharedKernel.Entities;
using NetApiCleanTemplate.SharedKernel.Guards;

namespace NetApiCleanTemplate.Core.General.Guards;

public static class GenericGuards
{
    public static void NonExistentEntity<TEntity, TPrimaryKey>(this IGuardClause _, TEntity? entity, TPrimaryKey id) 
        where TEntity : BaseEntity<TPrimaryKey>
    {
        if (entity == null)
        {
            throw new EntityNotFoundException<TEntity, TPrimaryKey>(id);
        }
    }
}

