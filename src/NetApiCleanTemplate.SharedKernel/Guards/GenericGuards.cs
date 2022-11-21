using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities;
using NetApiCleanTemplate.SharedKernel.Exceptions;
using NetApiCleanTemplate.SharedKernel.Guards;

namespace NetApiCleanTemplate.SharedKernel.Guards;

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

