using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Entities.Interfaces;
public interface IBaseEntity<T>
{
    T Id { get; set; }
}
public interface IBaseEntity : IBaseEntity<int> { }

