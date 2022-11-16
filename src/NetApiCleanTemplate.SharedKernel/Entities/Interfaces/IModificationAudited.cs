using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Entities.Interfaces;
public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; set; }
}

public interface IModificationAudited<T> : IHasModificationTime
{
    T? LastModifierUserId { get; set; }
}
public interface IModificationAudited : IModificationAudited<string> { }
