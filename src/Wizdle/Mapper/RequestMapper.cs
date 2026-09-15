namespace Wizdle.Mapper;

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Extensions.Logging;

using Wizdle.Models;
using Wizdle.Solver;

internal sealed partial class RequestMapper : IRequestMapper
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
            LogNullRequest(_logger, nameof(WizdleRequest), nameof(SolveParameters));
            return new SolveParameters();
        }

        LogMappingRequest(
            _logger,
            nameof(WizdleRequest),
            request.CorrectLetters,
            request.MisplacedLetters,
            request.ExcludeLetters);

        var solveParameters = new SolveParameters();

        for (int i = 0; i < MaxWordLength; i++)
        {
            solveParameters.CorrectLetters.Add(MapLetterAtPosition(request.CorrectLetters, i));
            solveParameters.MisplacedLetters.Add(MapLetterAtPosition(request.MisplacedLetters, i));
        }

        foreach (char letter in request.ExcludeLetters)
        {
            if (char.IsLetter(letter)
                && !solveParameters.ExcludeLetters.Contains(char.ToLower(letter, CultureInfo.InvariantCulture)))
            {
                solveParameters.ExcludeLetters.Add(char.ToLower(letter, CultureInfo.InvariantCulture));
            }
        }

        LogMappedParameters(
            _logger,
            nameof(SolveParameters),
            solveParameters.CorrectLetters,
            solveParameters.MisplacedLetters,
            solveParameters.ExcludeLetters);

        return solveParameters;
    }

    private static char MapLetterAtPosition(string letters, int index)
    {
        if (index < letters.Length && char.IsLetter(letters[index]))
        {
            return char.ToLower(letters[index], CultureInfo.InvariantCulture);
        }

        return '?';
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Received null {RequestType}, returning default {ParametersType}")]
    static partial void LogNullRequest(
        ILogger logger,
        string requestType,
        string parametersType);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Mapping {RequestType}: [CorrectLetters: \"{CorrectLetters}\", MisplacedLetters: \"{MisplacedLetters}\", ExcludeLetters: \"{ExcludeLetters}\"]")]
    static partial void LogMappingRequest(
        ILogger logger,
        string requestType,
        string correctLetters,
        string misplacedLetters,
        string excludeLetters);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "Mapped {ParametersType}: [CorrectLetters: \"{CorrectLetters}\", MisplacedLetters: \"{MisplacedLetters}\", ExcludeLetters: \"{ExcludeLetters}\"]")]
    static partial void LogMappedParameters(
        ILogger logger,
        string parametersType,
        List<char> correctLetters,
        List<char> misplacedLetters,
        List<char> excludeLetters);
}
