namespace Wizdle.Models
{
    public class Request
    {
        public string CorrectLetters { get; init; } = string.Empty;

        public string MisplacedLetters { get; init; } = string.Empty;

        public string ExcludedLetters { get; init; } = string.Empty;
    }
}