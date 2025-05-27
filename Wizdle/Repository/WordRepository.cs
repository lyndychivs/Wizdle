namespace Wizdle.Repository
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Wizdle.File;

    internal class WordRepository : IWordRepository
    {
        private readonly ILogger _logger;

        private readonly IWordFile _wordFile;

        internal WordRepository(ILogger logger)
            : this(logger, new WordFile(logger))
        {
        }

        internal WordRepository(ILogger logger, IWordFile wordFile)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wordFile = wordFile ?? throw new ArgumentNullException(nameof(wordFile));
        }

        public IEnumerable<string> GetWords()
        {
            foreach (string word in _wordFile.ReadLines())
            {
                if (string.IsNullOrWhiteSpace(word))
                {
                    _logger.LogWarning("Found NullOrWhiteSpace in Word file, skipping.");

                    continue;
                }

                string response = word.ToLower().Trim();

                if (response.Length != 5)
                {
                    _logger.LogWarning($"Found Word with length {response.Length} in Word file, skipping: {response}");

                    continue;
                }

                yield return response;
            }
        }
    }
}