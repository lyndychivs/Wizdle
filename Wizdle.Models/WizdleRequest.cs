namespace Wizdle.Models
{
    /// <summary>
    /// The request model for the Wizdle library.
    /// </summary>
    public class WizdleRequest
    {
        /// <summary>
        /// Gets or sets correct Letters known to exist in the Word,
        /// Follow the format of "a.b.c" where unknown letters are represented by a dot (.).
        /// </summary>
        public string CorrectLetters { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets misplaced Letters known to exist in the Word,
        /// Follow the format of "a.b.c" where unknown letters are represented by a dot (.).
        /// </summary>
        public string MisplacedLetters { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets letters that are known to not exist in the Word,
        /// Follow the format of "abc" where each letter is a single character.
        /// </summary>
        public string ExcludeLetters { get; set; } = string.Empty;
    }
}