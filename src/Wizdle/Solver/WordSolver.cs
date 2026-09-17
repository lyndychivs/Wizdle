namespace Wizdle.Solver;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Wizdle.Repository;
using Wizdle.Validator;

/// <summary>
/// Solves for words matching the given <see cref="SolveParameters"/>.
/// </summary>
internal sealed partial class WordSolver : IWordSolver
{
    private readonly ILogger _logger;

    private readonly IWordRepository _wordRepository;

    private readonly ISolveParametersValidator _wordParameterValidator;

    private readonly IReadOnlyList<string> _words;

    /// <summary>
    /// Initializes a new instance of the <see cref="WordSolver"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> interface to use.</param>
    internal WordSolver(ILogger logger)
        : this(logger, new WordRepository(logger), new SolveParametersValidator(logger))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WordSolver"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> interface to use.</param>
    /// <param name="wordRepository">The <see cref="IWordRepository"/> to source words from.</param>
    /// <param name="wordParameterValidator">The <see cref="ISolveParametersValidator"/> to validate parameters with.</param>
    internal WordSolver(ILogger logger, IWordRepository wordRepository, ISolveParametersValidator wordParameterValidator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
        _wordParameterValidator = wordParameterValidator ?? throw new ArgumentNullException(nameof(wordParameterValidator));

        _words = [.. _wordRepository.GetWords()];

        if (_words.Any() is false)
        {
            LogNoWords(_logger, nameof(IWordRepository));
        }
    }

    /// <inheritdoc/>
    public IEnumerable<string> Solve(SolveParameters solveParameters)
    {
        if (_wordParameterValidator.IsValid(solveParameters) is false)
        {
            LogInvalidParametersReturningEmpty(_logger, nameof(SolveParameters));

            return [];
        }

        if (_words.Any() is false)
        {
            LogNoWordsReturningEmpty(_logger, nameof(IWordRepository));

            return [];
        }

        return FilterCorrectAndMisplacedLetters(
            FilterExcludeLetters(_words, solveParameters.ExcludeLetters),
            solveParameters.CorrectLetters,
            solveParameters.MisplacedLetters);
    }

    /// <summary>
    /// Filters the given words to those matching the correct and misplaced letter criteria.
    /// </summary>
    /// <param name="wordsToFilter">The words to filter.</param>
    /// <param name="correctLetters">The letters known to be correct at their position.</param>
    /// <param name="misplacedLetters">The letters known to be present but misplaced.</param>
    /// <returns>The filtered words.</returns>
    private static List<string> FilterCorrectAndMisplacedLetters(List<string> wordsToFilter, List<char> correctLetters, List<char> misplacedLetters)
    {
        if (wordsToFilter.Count == 0)
        {
            return [];
        }

        List<string> filteredWords = wordsToFilter;
        for (int i = 0; i < correctLetters.Count; i++)
        {
            char correctLetter = correctLetters[i];
            char misplacedLetter = misplacedLetters[i];

            if (correctLetter == '?' && misplacedLetter == '?')
            {
                continue;
            }

            foreach (string word in filteredWords.ToList())
            {
                if (correctLetter != '?')
                {
                    if (word[i] != correctLetter)
                    {
                        filteredWords.Remove(word);
                        continue;
                    }
                }

                if (misplacedLetter != '?')
                {
                    if (!word.Contains(misplacedLetter))
                    {
                        filteredWords.Remove(word);
                        continue;
                    }

                    if (word[i] == misplacedLetter)
                    {
                        filteredWords.Remove(word);
                    }
                }
            }
        }

        return filteredWords;
    }

    /// <summary>
    /// Filters the given words to exclude those containing any of the given letters.
    /// </summary>
    /// <param name="wordsToFilter">The words to filter.</param>
    /// <param name="excludeLetters">The letters to exclude.</param>
    /// <returns>The filtered words.</returns>
    private static List<string> FilterExcludeLetters(IEnumerable<string> wordsToFilter, List<char> excludeLetters)
    {
        return [.. wordsToFilter.Where(word => !excludeLetters.Exists(letter => word.Contains(letter)))];
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "No Words returned from {RepositoryType}")]
    static partial void LogNoWords(
        ILogger logger,
        string repositoryType);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "{ParametersType} is not valid, returning empty")]
    static partial void LogInvalidParametersReturningEmpty(
        ILogger logger,
        string parametersType);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "No Words returned from {RepositoryType}, returning empty")]
    static partial void LogNoWordsReturningEmpty(
        ILogger logger,
        string repositoryType);
}
