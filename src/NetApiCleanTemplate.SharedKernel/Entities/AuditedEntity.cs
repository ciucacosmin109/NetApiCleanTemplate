using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.SharedKernel.Entities;

public abstract class AuditedEntity<T> : BaseEntity<T>, IAudited<T>
{
    public T? CreatorUserId { get; set; }
    public DateTime CreationTime { get; set; }

    public T? LastModifierUserId { get; set; }
    public DateTime? LastModificationTime { get; set; }
}

public abstract class AuditedEntity : AuditedEntity<int> { }
