using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Entities.Interfaces;

namespace NetApiCleanTemplate.SharedKernel.Entities;

public abstract class FullyAuditedEntity<T> : AuditedEntity<T>, IFullyAudited<T>
{
    public T? DeleterUserId { get; set; }
    public DateTime? DeletionTime { get; set; }
    public bool IsDeleted { get; set; }

    public FullyAuditedEntity(T id) : base(id) { }
}

public abstract class FullyAuditedEntity : FullyAuditedEntity<int>
{
    public FullyAuditedEntity(int id) : base(id) { }
}
