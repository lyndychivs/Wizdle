namespace Wizdle.Solver;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Wizdle.Repository;
using Wizdle.Validator;

internal class WordSolver : IWordSolver
{
    private readonly ILogger _logger;

    private readonly IWordRepository _wordRepository;

    private readonly ISolveParametersValidator _wordParameterValidator;

    private readonly IEnumerable<string> _words;

    internal WordSolver(ILogger logger)
        : this(logger, new WordRepository(logger), new SolveParametersValidator(logger))
    {
    }

    internal WordSolver(ILogger logger, IWordRepository wordRepository, ISolveParametersValidator wordParameterValidator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
        _wordParameterValidator = wordParameterValidator ?? throw new ArgumentNullException(nameof(wordParameterValidator));

        _words = _wordRepository.GetWords();

        if (_words.Any() is false)
        {
            _logger.LogError($"No Words returned from {nameof(IWordRepository)}");
        }
    }

    public IEnumerable<string> Solve(SolveParameters solveParameters)
    {
        if (_wordParameterValidator.IsValid(solveParameters) is false)
        {
            _logger.LogWarning($"{nameof(SolveParameters)} is not valid, returning empty");

            return [];
        }

        if (_words.Any() is false)
        {
            _logger.LogError($"No Words returned from {nameof(IWordRepository)}, returning empty");

            return [];
        }

        return FilterCorrectAndMisplacedLetters(
            FilterExcludeLetters(_words, solveParameters.ExcludeLetters),
            solveParameters.CorrectLetters,
            solveParameters.MisplacedLetters);
    }

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

    private static List<string> FilterExcludeLetters(IEnumerable<string> wordsToFilter, List<char> excludeLetters)
    {
        return [.. wordsToFilter.Where(word => !excludeLetters.Any(letter => word.Contains(letter)))];
    }
}
