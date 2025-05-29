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

        public SolveParameters MapToSolveParameters(WizdleRequest request)
        {
            if (request is null)
            {
                _logger.LogError($"Received null {nameof(WizdleRequest)}, returning default {nameof(SolveParameters)}");

                return new SolveParameters();
            }

            _logger.LogInformation(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-25} {1,-25} {2,-25} {3}",
                    $"Mapping {nameof(WizdleRequest)}:",
                    $"{nameof(WizdleRequest.CorrectLetters)}: \"{request.CorrectLetters}\"",
                    $"{nameof(WizdleRequest.MisplacedLetters)}: \"{request.MisplacedLetters}\"",
                    $"{nameof(WizdleRequest.ExcludeLetters)}: \"{request.ExcludeLetters}\""));

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

            foreach (char letter in request.ExcludeLetters)
            {
                if (char.IsLetter(letter)
                    && !solveParameters.ExcludeLetters.Contains(char.ToLower(letter, CultureInfo.InvariantCulture)))
                {
                    solveParameters.ExcludeLetters.Add(char.ToLower(letter, CultureInfo.InvariantCulture));
                }
            }

            _logger.LogInformation(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-25} {1}",
                    $"Mapped {nameof(SolveParameters)}:",
                    $"{solveParameters}"));

            return solveParameters;
        }
    }
}