namespace Wizdle.Mapper
{
    using System;
    using System.Globalization;

    using Microsoft.Extensions.Logging;

    using Wizdle.Models;
    using Wizdle.Solver;

    internal class RequestMapper : IRequestMapper
    {
        private const int MaxWordLength = 5;

        private readonly ILogger _logger;

        internal RequestMapper(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public SolveParameters MapToSolveParameters(Request request)
        {
            if (request is null)
            {
                _logger.LogWarning($"Received null {nameof(Request)}, returning default {nameof(SolveParameters)}");

                return new SolveParameters();
            }

            _logger.LogInformation(
                $"Mapping {nameof(Request)}:"
                + $"\n{nameof(Request.CorrectLetters)}: \"{request.CorrectLetters}\""
                + $"\n{nameof(Request.MisplacedLetters)}: \"{request.MisplacedLetters}\""
                + $"\n{nameof(Request.ExcludedLetters)}: \"{request.ExcludedLetters}\"");

            var solveParameters = new SolveParameters();

            for (int i = 0; i < MaxWordLength; i++)
            {
                if (i < request.CorrectLetters.Length)
                {
                    if (char.IsLetter(request.CorrectLetters[i]))
                    {
                        solveParameters.CorrectLetters.Add(char.ToLower(request.CorrectLetters[i], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        solveParameters.CorrectLetters.Add('?');
                    }
                }
                else
                {
                    solveParameters.CorrectLetters.Add('?');
                }

                if (i < request.MisplacedLetters.Length)
                {
                    if (char.IsLetter(request.MisplacedLetters[i]))
                    {
                        solveParameters.MisplacedLetters.Add(char.ToLower(request.MisplacedLetters[i], CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        solveParameters.MisplacedLetters.Add('?');
                    }
                }
                else
                {
                    solveParameters.MisplacedLetters.Add('?');
                }
            }

            solveParameters.ExcludeLetters = request.ExcludedLetters.ToLower(CultureInfo.InvariantCulture).ToCharArray();

            _logger.LogInformation($"Mapped {nameof(SolveParameters)}:\n{solveParameters}");

            return solveParameters;
        }
    }
}