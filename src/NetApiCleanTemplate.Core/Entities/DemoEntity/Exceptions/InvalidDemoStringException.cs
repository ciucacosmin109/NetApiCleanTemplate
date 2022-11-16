using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Constants;
using NetApiCleanTemplate.SharedKernel.Exceptions;

namespace NetApiCleanTemplate.Core.Entities.DemoEntity.Exceptions;

public class InvalidDemoStringException : DomainException
{
    public InvalidDemoStringException() : base($"Invalid demo string (not null, max length: {DemoConstants.MaxDemoStringLength})") { }
}

