namespace Wizdle.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.Extensions.Logging;

    using Wizdle.Words;

    internal class WordRepository : IWordRepository
    {
        private readonly ILogger _logger;

        private readonly IWords _words;

        internal WordRepository(ILogger logger)
            : this(logger, new Words())
        {
        }

        internal WordRepository(ILogger logger, IWords words)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _words = words ?? throw new ArgumentNullException(nameof(words));
        }

        public IEnumerable<string> GetWords()
        {
            foreach (string word in _words.GetWords())
            {
                if (string.IsNullOrWhiteSpace(word))
                {
                    _logger.LogWarning("Found NullOrWhiteSpace in Word file, skipping.");

                    continue;
                }

                string response = word.ToLower(CultureInfo.InvariantCulture).Trim();

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