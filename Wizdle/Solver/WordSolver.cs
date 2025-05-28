namespace Wizdle.Solver
{
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

        private readonly IEnumerable<string> _defaultResponse = ["hates", "round", "climb"];

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

            if (_words.Any() == false)
            {
                _logger.LogError($"No Words returned from {nameof(IWordRepository)}");
            }
        }

        public IEnumerable<string> Solve(SolveParameters solveParameters)
        {
            ValidatorResponse validatorResponse = _wordParameterValidator.IsValid(solveParameters);
            if (validatorResponse.IsValid == false)
            {
                _logger.LogWarning($"{nameof(SolveParameters)} is not valid, returning {nameof(_defaultResponse)}");

                return _defaultResponse;
            }

            if (_words.Any() == false)
            {
                _logger.LogError($"No Words returned from {nameof(IWordRepository)}, returning {nameof(_defaultResponse)}");

                return _defaultResponse;
            }

            return FilterCorrectAndMisplacedLetters(
                FilterExcludeLetters(_words, solveParameters.ExcludeLetters),
                solveParameters.CorrectLetters,
                solveParameters.MisplacedLetters);
        }

        private static IEnumerable<string> FilterCorrectAndMisplacedLetters(IEnumerable<string> wordsToFilter, IList<char> correctLetters, IList<char> misplacedLetters)
        {
            if (wordsToFilter.Any() == false)
            {
                return [];
            }

            IList<string> filteredWords = [..wordsToFilter];
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

        private static IEnumerable<string> FilterExcludeLetters(IEnumerable<string> wordsToFilter, IList<char> excludeLetters)
        {
            return wordsToFilter.Where(word => !excludeLetters.Any(letter => word.Contains(letter)));
        }
    }
}