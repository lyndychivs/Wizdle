namespace Wizdle.Solver;

using System.Collections.Generic;

internal sealed class SolveParameters
{
    public List<char> CorrectLetters { get; set; } = [];

    public List<char> MisplacedLetters { get; set; } = [];

    public List<char> ExcludeLetters { get; set; } = [];
}
