namespace Wizdle.Mapper;

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Extensions.Logging;

using Wizdle.Models;
using Wizdle.Solver;

/// <summary>
/// Maps a <see cref="WizdleRequest"/> to a <see cref="SolveParameters"/>.
/// </summary>
internal sealed partial class RequestMapper : IRequestMapper
{
    private const int MaxWordLength = 5;

    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestMapper"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> interface to use.</param>
    internal RequestMapper(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Maps the letter at the given index, returning '?' if the index is out of range or the character is not a letter.
    /// </summary>
    /// <param name="letters">The letters to map from.</param>
    /// <param name="index">The index of the letter to map.</param>
    /// <returns>The lower-cased letter, or '?' if not applicable.</returns>
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
