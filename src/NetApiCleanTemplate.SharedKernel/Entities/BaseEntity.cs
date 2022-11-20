using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.SharedKernel.Entities;

public abstract class BaseEntity<T> : IBaseEntity<T>
{
    public T Id { get; set; } = default!;
}

public abstract class BaseEntity : BaseEntity<int> { }
