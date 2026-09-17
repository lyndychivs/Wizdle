namespace Wizdle.Mapper;

using Wizdle.Models;
using Wizdle.Solver;

/// <summary>
/// Maps a <see cref="WizdleRequest"/> to a <see cref="SolveParameters"/>.
/// </summary>
internal interface IRequestMapper
{
    /// <summary>
    /// Maps the given <see cref="WizdleRequest"/> to a <see cref="SolveParameters"/>.
    /// </summary>
    /// <param name="request">The request to map.</param>
    /// <returns>The mapped <see cref="SolveParameters"/>.</returns>
    SolveParameters MapToSolveParameters(WizdleRequest request);
}
