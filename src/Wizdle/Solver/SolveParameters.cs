namespace Wizdle.Solver;

using System.Collections.Generic;

/// <summary>
/// The criteria used when solving for matching words.
/// </summary>
internal sealed class SolveParameters
{
    /// <summary>
    /// Gets or sets the letters known to be correct at their position.
    /// </summary>
    public List<char> CorrectLetters { get; set; } = [];

    /// <summary>
    /// Gets or sets the letters known to be present but misplaced.
    /// </summary>
    public List<char> MisplacedLetters { get; set; } = [];

    /// <summary>
    /// Gets or sets the letters known to be excluded.
    /// </summary>
    public List<char> ExcludeLetters { get; set; } = [];
}
