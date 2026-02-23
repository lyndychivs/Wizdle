namespace Wizdle.Repository;

using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Extensions.Logging;

using Wizdle.Words;

internal sealed partial class WordRepository : IWordRepository
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
                LogNullOrWhiteSpaceWord(_logger);

                continue;
            }

            string response = word.ToLower(CultureInfo.InvariantCulture).Trim();

            if (response.Length != 5)
            {
                LogInvalidWordLength(_logger, response.Length, response);

                continue;
            }

            yield return response;
        }
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Warning,
        Message = "Found NullOrWhiteSpace in Words, skipping")]
    static partial void LogNullOrWhiteSpaceWord(ILogger logger);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Warning,
        Message = "Found Word with length {Length} in Words, skipping: {Word}")]
    static partial void LogInvalidWordLength(
        ILogger logger,
        int length,
        string word);
}
