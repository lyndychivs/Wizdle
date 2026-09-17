namespace Wizdle.Solver;

using System.Collections.Generic;

/// <summary>
/// Solves for words matching the given <see cref="SolveParameters"/>.
/// </summary>
internal interface IWordSolver
{
    /// <summary>
    /// Solves for words matching the given <see cref="SolveParameters"/>.
    /// </summary>
    /// <param name="solveParameters">The criteria to solve with.</param>
    /// <returns>The matching words.</returns>
    IEnumerable<string> Solve(SolveParameters solveParameters);
}
