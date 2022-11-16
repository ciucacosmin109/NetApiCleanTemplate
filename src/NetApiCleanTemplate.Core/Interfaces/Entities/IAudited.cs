using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Interfaces.Entities;
public interface IAudited<T> : ICreationAudited<T>, IModificationAudited<T>
{
}
public interface IAudited : IAudited<string> { }

