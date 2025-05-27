namespace Wizdle.Validator
{
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

        public ValidatorResponse IsValid(SolveParameters solveParameters)
        {
            var validatorResponse = new ValidatorResponse();
            if (solveParameters is null)
            {
                string message = $"{nameof(SolveParameters)} is null";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);

                return validatorResponse;
            }

            if (solveParameters.CorrectLetters.Count != 5)
            {
                string message = $"{nameof(SolveParameters)}.{nameof(SolveParameters.CorrectLetters)} Letter count is not equal to 5";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
            }

            if (solveParameters.MisplacedLetters.Count != 5)
            {
                string message = $"{nameof(SolveParameters)}.{nameof(SolveParameters.MisplacedLetters)} Letter count is not equal to 5";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
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
                    string message = $"{nameof(SolveParameters.CorrectLetters)} and {nameof(SolveParameters.MisplacedLetters)} contain the same letter at index {i}, Letter: '{solveParameters.CorrectLetters[i]}'";
                    _logger.LogDebug(message);

                    validatorResponse.IsValid = false;
                    validatorResponse.Errors.Add(message);
                }
            }

            foreach (char c in solveParameters.ExcludeLetters)
            {
                if (solveParameters.CorrectLetters.Contains(c) || solveParameters.MisplacedLetters.Contains(c))
                {
                    string message = $"{nameof(SolveParameters.ExcludeLetters)} contains a letter that exists in {nameof(SolveParameters.CorrectLetters)} or {nameof(SolveParameters.MisplacedLetters)}, Letter: '{c}'";
                    _logger.LogDebug(message);

                    validatorResponse.IsValid = false;
                    validatorResponse.Errors.Add(message);
                }
            }

            return validatorResponse;
        }
    }
}