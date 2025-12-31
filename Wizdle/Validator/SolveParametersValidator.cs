namespace Wizdle.Validator;

using System;

using Microsoft.Extensions.Logging;

using Wizdle.Solver;

internal class SolveParametersValidator : ISolveParametersValidator
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
            _logger.LogDebug($"{nameof(SolveParameters)} cannot be null");
            return false;
        }

        if (solveParameters.CorrectLetters.Count != 5)
        {
            isValid = false;
            _logger.LogDebug($"{nameof(SolveParameters)}.{nameof(SolveParameters.CorrectLetters)} Letter count is not equal to 5");
        }

        if (solveParameters.MisplacedLetters.Count != 5)
        {
            isValid = false;
            _logger.LogDebug($"{nameof(SolveParameters)}.{nameof(SolveParameters.MisplacedLetters)} Letter count is not equal to 5");
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
                _logger.LogDebug($"{nameof(SolveParameters.CorrectLetters)} and {nameof(SolveParameters.MisplacedLetters)} contain the same letter at index {i}, Letter: '{solveParameters.CorrectLetters[i]}'");
            }
        }

        foreach (char c in solveParameters.ExcludeLetters)
        {
            if (solveParameters.CorrectLetters.Contains(c) || solveParameters.MisplacedLetters.Contains(c))
            {
                isValid = false;
                _logger.LogDebug($"{nameof(SolveParameters.ExcludeLetters)} contains a letter that exists in {nameof(SolveParameters.CorrectLetters)} or {nameof(SolveParameters.MisplacedLetters)}, Letter: '{c}'");
            }
        }

        return isValid;
    }
}
