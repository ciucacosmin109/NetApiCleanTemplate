using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.SharedKernel.Guards;

/// <summary>
/// Simple interface to provide a generic mechanism to build guard clause extension methods from.
/// </summary>
public interface IGuardClause { }
internal class GuardClause : IGuardClause { }

/// <summary>
/// An entry point (Guard.Against) to a set of Guard Clauses defined as extension methods on IGuardClause.
/// </summary>
/// <remarks>See http://www.weeklydevtips.com/004 on Guard Clauses</remarks>
public class Guard
{
    public static IGuardClause Against { get; } = new GuardClause();
}
