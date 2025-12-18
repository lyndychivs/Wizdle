namespace Wizdle.Console;

using CommandLine;

[Verb("solve", true, ["s", "S"], HelpText = "Attempts to guess the Word by filtering from the provided arguments.")]
internal sealed class SolveArguments
{
    [Option('c', "correct", Required = false, HelpText = "Correct Letters known to exist in the Word.\nFollow the format of \"a.b.c\" where unknown letters are represented by a dot (.).")]
    public string CorrectLetters { get; set; } = string.Empty;

    [Option('m', "misplaced", Required = false, HelpText = "Misplaced Letters known to exist in the Word.\nFollow the format of \"a.b.c\" where unknown letters are represented by a dot (.).")]
    public string MisplacedLetters { get; set; } = string.Empty;

    [Option('e', "exclude", Required = false, HelpText = "Letters that are known to not exist in the Word.\nFollow the format of \"abc\" where each letter is a single character.")]
    public string ExcludeLetters { get; set; } = string.Empty;
}
