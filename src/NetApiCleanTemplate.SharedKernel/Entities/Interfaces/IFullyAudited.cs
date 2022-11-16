using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Entities.Interfaces;
public interface IFullyAudited<T> : IAudited<T>, IDeletionAudited<T>
{
}
public interface IFullyAudited : IFullyAudited<string> { }
