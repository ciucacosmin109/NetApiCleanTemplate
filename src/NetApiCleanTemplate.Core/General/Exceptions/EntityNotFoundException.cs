using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Exceptions;

namespace NetApiCleanTemplate.Core.General.Exceptions;
public class EntityNotFoundException<TEntity, TPrimaryKey> : DomainException
{
    public EntityNotFoundException(TPrimaryKey id) : base($"There is no [{typeof(TEntity).Name}] with Id={id}") { }
}

