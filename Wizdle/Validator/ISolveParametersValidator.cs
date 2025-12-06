namespace Wizdle.Validator;

using Wizdle.Solver;

internal interface ISolveParametersValidator
{
    ValidatorResponse IsValid(SolveParameters solveParameters);
}
