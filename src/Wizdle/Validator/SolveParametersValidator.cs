namespace Wizdle.Validator;

using System;

using Microsoft.Extensions.Logging;

using Wizdle.Solver;

internal sealed partial class SolveParametersValidator : ISolveParametersValidator
{
    private readonly ILogger _logger;

    internal SolveParametersValidator(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public bool IsValid(SolveParameters solveParameters)
    {
        bool isValid = true;

        if (solveParameters is null)
        {
            LogParameterCannotBeNull(_logger, nameof(solveParameters));
            return false;
        }

        if (solveParameters.CorrectLetters.Count != 5)
        {
            isValid = false;
            LogParameterWithInvalidLettersCount(_logger, nameof(solveParameters.CorrectLetters));
        }

        if (solveParameters.MisplacedLetters.Count != 5)
        {
            isValid = false;
            LogParameterWithInvalidLettersCount(_logger, nameof(solveParameters.MisplacedLetters));
        }

        for (int i = 0; i < Math.Min(solveParameters.CorrectLetters.Count, 5); i++)
        {
            if (solveParameters.CorrectLetters[i] == '?')
            {
                continue;
            }

            if (i >= solveParameters.MisplacedLetters.Count)
            {
                continue;
            }

            if (solveParameters.CorrectLetters[i] == solveParameters.MisplacedLetters[i])
            {
                isValid = false;
                LogDuplicateLetterAtIndex(_logger, i, solveParameters.CorrectLetters[i]);
            }
        }

        foreach (char c in solveParameters.ExcludeLetters)
        {
            if (solveParameters.CorrectLetters.Contains(c) || solveParameters.MisplacedLetters.Contains(c))
            {
                isValid = false;
                LogExcludedLetterConflict(_logger, c);
            }
        }

        return isValid;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "{ParameterName} cannot be null")]
    static partial void LogParameterCannotBeNull(ILogger logger, string parameterName);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "{ParameterName} Letter count is not equal to 5")]
    static partial void LogParameterWithInvalidLettersCount(ILogger logger, string parameterName);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "CorrectLetters and MisplacedLetters contain the same letter at index {Index}, Letter: '{Letter}'")]
    static partial void LogDuplicateLetterAtIndex(ILogger logger, int index, char letter);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Debug,
        Message = "ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: '{Letter}'")]
    static partial void LogExcludedLetterConflict(ILogger logger, char letter);
}
