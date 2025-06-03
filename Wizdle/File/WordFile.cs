namespace Wizdle.File
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Microsoft.Extensions.Logging;

    internal class WordFile : IWordFile
    {
        private const string WordFilePath = "Source\\words.txt";

        private readonly ILogger _logger;

        internal WordFile(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<string> ReadLines()
        {
            try
            {
                string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory(), WordFilePath);

                if (File.Exists(filePath) == false)
                {
                    _logger.LogError($"File does not exist at Path: {filePath}");

                    return new List<string>();
                }

                return File.ReadLines(filePath);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to read content from File");

                return new List<string>();
            }
        }
    }
}