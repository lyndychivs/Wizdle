namespace Wizdle.Mapper;

using Wizdle.Models;
using Wizdle.Solver;

internal interface IRequestMapper
{
    SolveParameters MapToSolveParameters(WizdleRequest request);
}
