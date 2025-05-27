namespace Wizdle.Tests.Repository
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.File;
    using Wizdle.Repository;

    [TestFixture]
    public class WordRepositoryTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly Mock<IWordFile> _wordFileMock;

        private readonly WordRepository _wordRepository;

        public WordRepositoryTests()
        {
            _loggerMock = new Mock<ILogger>();
            _wordFileMock = new Mock<IWordFile>();
            _wordRepository = new WordRepository(_loggerMock.Object, _wordFileMock.Object);
        }

        [Test]
        public void GetWords_WordFileContainsWordWithUpperAndWhitespace_ReturnsWordsLowercaseAndTrimmed()
        {
            // Arrange
            var inputWords = new List<string>
            {
                "aaaaa",
                "  BBBBB ",
                "EEEEE",
            };

            _wordFileMock.Setup(f => f.ReadLines()).Returns(inputWords);

            // Act
            IEnumerable<string> result = _wordRepository.GetWords();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(["aaaaa", "bbbbb", "eeeee"]));
        }

        [Test]
        public void GetWords_WordFileContainsOnlyNullEmptyOrWhitespace_ReturnsEmptyAndLogs()
        {
            // Arrange
            List<string> inputWords = [string.Empty, " ", null];
            _wordFileMock.Setup(f => f.ReadLines()).Returns(inputWords);

            // Act
            IEnumerable<string> results = _wordRepository.GetWords();

            // Assert
            Assert.That(results, Is.Empty);
            _loggerMock.VerifyLogging(
                "Found NullOrWhiteSpace in Word file, skipping.",
                LogLevel.Warning,
                Times.Exactly(3));
        }

        [TestCase("a")]
        [TestCase("dddd")]
        [TestCase("eeeeee")]
        public void GetWords_WordFileContainsOnlyInvalidStringLengths_ReturnsEmptyAndLogs(string word)
        {
            // Arrange
            List<string> inputWords = [word];
            _wordFileMock.Setup(f => f.ReadLines()).Returns(inputWords);

            // Act
            IEnumerable<string> results = _wordRepository.GetWords();

            // Assert
            Assert.That(results, Is.Empty);
            _loggerMock.VerifyLogging(
                $"Found Word with length {word.Length} in Word file, skipping: {word}",
                LogLevel.Warning,
                Times.Once());
        }

        [Test]
        public void GetWords_WordFileIsEmpty_ReturnsEmpty()
        {
            // Arrange
            _wordFileMock.Setup(f => f.ReadLines()).Returns(new List<string>());

            // Act
            IEnumerable<string> result = _wordRepository.GetWords();

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}