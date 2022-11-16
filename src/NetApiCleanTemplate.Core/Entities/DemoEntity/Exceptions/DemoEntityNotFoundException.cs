using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.SharedKernel.Exceptions;

namespace NetApiCleanTemplate.Core.Entities.DemoEntity.Exceptions;

public class DemoEntityNotFoundException : DomainException
{
    public DemoEntityNotFoundException(int id) : base($"Demo entity with id {id} was not found") { }
} 

