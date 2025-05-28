﻿namespace Wizdle.Solver
{
    using System.Collections.Generic;
    using System.Globalization;

    internal class SolveParameters
    {
        public IList<char> CorrectLetters { get; set; } = [];

        public IList<char> MisplacedLetters { get; set; } = [];

        public IList<char> ExcludeLetters { get; set; } = [];

        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0,-25} {1,-25} {2}",
                $"{nameof(CorrectLetters)}: \"{string.Join(string.Empty, CorrectLetters)}\"",
                $"{nameof(MisplacedLetters)}: \"{string.Join(string.Empty, MisplacedLetters)}\"",
                $"{nameof(ExcludeLetters)}: \"{string.Join(string.Empty, ExcludeLetters)}\"");
        }
    }
}