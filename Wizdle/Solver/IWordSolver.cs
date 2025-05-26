namespace Wizdle.Solver
{
    using System.Collections.Generic;

    internal interface IWordSolver
    {
        IEnumerable<string> Solve(SolveParameters solveParameters);
    }
}