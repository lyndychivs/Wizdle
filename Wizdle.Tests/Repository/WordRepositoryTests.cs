namespace Wizdle.Tests.Repository
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Repository;
    using Wizdle.Words;

    [TestFixture]
    public class WordRepositoryTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly Mock<IWords> _wordsMock;

        private readonly WordRepository _wordRepository;

        public WordRepositoryTests()
        {
            _loggerMock = new Mock<ILogger>();
            _wordsMock = new Mock<IWords>();
            _wordRepository = new WordRepository(_loggerMock.Object, _wordsMock.Object);
        }

        [Test]
        public void GetWords_WordsContainsWordWithUpperAndWhitespace_ReturnsWordsLowercaseAndTrimmed()
        {
            // Arrange
            var inputWords = new List<string>
            {
                "aaaaa",
                "  BBBBB ",
                "EEEEE",
            };

            _wordsMock.Setup(f => f.GetWords()).Returns(inputWords);

            // Act
            IEnumerable<string> result = _wordRepository.GetWords();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(["aaaaa", "bbbbb", "eeeee"]));
        }

        [Test]
        public void GetWords_WordsContainsOnlyNullEmptyOrWhitespace_ReturnsEmptyAndLogs()
        {
            // Arrange
            List<string> inputWords = [string.Empty, " ", null];
            _wordsMock.Setup(f => f.GetWords()).Returns(inputWords);

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
        public void GetWords_WordsContainsOnlyInvalidStringLengths_ReturnsEmptyAndLogs(string word)
        {
            // Arrange
            List<string> inputWords = [word];
            _wordsMock.Setup(f => f.GetWords()).Returns(inputWords);

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
        public void GetWords_WordsIsEmpty_ReturnsEmpty()
        {
            // Arrange
            _wordsMock.Setup(f => f.GetWords()).Returns([]);

            // Act
            IEnumerable<string> result = _wordRepository.GetWords();

            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}