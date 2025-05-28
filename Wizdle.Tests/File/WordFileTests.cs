namespace Wizdle.Tests.File
{
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.File;

    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class WordFileTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly string _filePath;

        public WordFileTests()
        {
            _loggerMock = new Mock<ILogger>();
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Source\\words.txt");
        }

        [Test]
        public void ReadLines_FileExists_ReturnsLines()
        {
            // Arrange
            DeleteFile();
            string[] lines = ["a", "b", "c"];
            File.WriteAllLines(_filePath, lines);
            var wordFile = new WordFile(_loggerMock.Object);

            // Act
            IEnumerable<string> result = wordFile.ReadLines();

            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(lines));
        }

        [Test]
        public void ReadLines_FileDoesNotExist_LogsErrorAndReturnsEmpty()
        {
            // Arrange
            DeleteFile();
            var wordFile = new WordFile(_loggerMock.Object);

            // Act
            IEnumerable<string> result = wordFile.ReadLines();

            // Assert
            Assert.That(result, Is.Empty);
            _loggerMock.VerifyLogging(
                $"File does not exist at Path: {_filePath}",
                LogLevel.Error,
                Times.Once());
        }

        [Test]
        public void ReadLines_ExceptionThrown_LogsErrorAndReturnsEmpty()
        {
            // Arrange
            DeleteFile();
            var wordFile = new WordFile(_loggerMock.Object);
            File.WriteAllText(_filePath, "test");
            using (FileStream fileStream = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                // Act
                IEnumerable<string> result = wordFile.ReadLines();

                // Assert
                Assert.That(result, Is.Empty);
                _loggerMock.VerifyLogging(
                    $"Failed to read content from File",
                    LogLevel.Error,
                    Times.Once());
            }
        }

        private void DeleteFile()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}