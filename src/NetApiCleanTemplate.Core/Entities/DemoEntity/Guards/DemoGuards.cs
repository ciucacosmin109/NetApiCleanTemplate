using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Constants;
using NetApiCleanTemplate.Core.Entities.DemoEntity.Exceptions;
using NetApiCleanTemplate.Core.Guards;

namespace NetApiCleanTemplate.Core.Entities.DemoEntity.Guards;

public static class DemoGuards
{
    public static void InvalidDemoString(this IGuardClause _, string demoString)
    {
        if (demoString == null)
        {
            throw new InvalidDemoStringException();
        }

        if (demoString.Length > DemoConstants.MaxDemoStringLength)
        {
            throw new InvalidDemoStringException();
        }
    }
}

