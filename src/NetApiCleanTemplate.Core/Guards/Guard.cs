using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApiCleanTemplate.Core.Guards;

/// <summary>
/// Simple interface to provide a generic mechanism to build guard clause extension methods from.
/// </summary>
public interface IGuardClause { }

/// <summary>
/// An entry point to a set of Guard Clauses defined as extension methods on IGuardClause.
/// </summary>
/// <remarks>See http://www.weeklydevtips.com/004 on Guard Clauses</remarks>
public class Guard : IGuardClause
{
    /// <summary>
    /// An entry point to a set of Guard Clauses.
    /// </summary>
    public static IGuardClause Against { get; } = new Guard();

    private Guard() { }
}
