namespace Wizdle.Validator;

using Wizdle.Solver;

/// <summary>
/// Validates a <see cref="SolveParameters"/>.
/// </summary>
internal interface ISolveParametersValidator
{
    /// <summary>
    /// Determines whether the given <see cref="SolveParameters"/> is valid.
    /// </summary>
    /// <param name="solveParameters">The parameters to validate.</param>
    /// <returns><see langword="true"/> if valid; otherwise, <see langword="false"/>.</returns>
    bool IsValid(SolveParameters solveParameters);
}
