using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Interfaces.Entities;
public interface IFullyAudited<T> : IAudited<T>, IDeletionAudited<T>
{
}
public interface IFullyAudited : IFullyAudited<string> { }
