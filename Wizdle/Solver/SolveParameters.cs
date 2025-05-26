namespace Wizdle.Solver
{
    using System.Collections.Generic;

    internal class SolveParameters
    {
        public IList<char> CorrectLetters { get; set; } = [];

        public IList<char> MisplacedLetters { get; set; } = [];

        public IList<char> ExcludeLetters { get; set; } = [];

        public override string ToString()
        {
            return $"{nameof(CorrectLetters)}: \"{string.Join(string.Empty, CorrectLetters)}\"\n{nameof(MisplacedLetters)}: \"{string.Join(string.Empty, MisplacedLetters)}\"\n{nameof(ExcludeLetters)}: \"{string.Join(string.Empty, ExcludeLetters)}\"";
        }
    }
}