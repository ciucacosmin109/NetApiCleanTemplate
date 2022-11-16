using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Interfaces.Entities; 
public interface IHasCreationTime
{
    DateTime CreationTime { get; set; }
}
public interface ICreationAudited<T> : IHasCreationTime
{
    T? CreatorUserId { get; set; }
}
public interface ICreationAudited : ICreationAudited<string> { }

