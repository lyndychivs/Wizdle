namespace Wizdle.Validator;

using Wizdle.Solver;

internal interface ISolveParametersValidator
{
    bool IsValid(SolveParameters solveParameters);
}
