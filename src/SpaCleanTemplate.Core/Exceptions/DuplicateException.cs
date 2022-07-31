using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaCleanTemplate.Core.Exceptions;

public class DuplicateException : DomainException
{
    public DuplicateException(string message) : base(message)
    {

    }
}
