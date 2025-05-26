namespace Wizdle.Tests.Mapper
{
    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Mapper;
    using Wizdle.Models;
    using Wizdle.Solver;

    [TestFixture]
    public class RequestMapperTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly RequestMapper _requestMapper;

        public RequestMapperTests()
        {
            _loggerMock = new Mock<ILogger>();
            _requestMapper = new RequestMapper(_loggerMock.Object);
        }

        [Test]
        public void MapToSolveParameters_ValidRequest_MapsCorrectly()
        {
            var request = new Request
            {
                CorrectLetters = "a....",
                MisplacedLetters = "b....",
                ExcludedLetters = "c",
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
                Assert.That(result.MisplacedLetters, Is.EqualTo(['b', '?', '?', '?', '?']));
                Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));

                _loggerMock.VerifyLogging(
                    "Mapping Request:"
                    + "\nCorrectLetters: \"a....\""
                    + "\nMisplacedLetters: \"b....\""
                    + "\nExcludedLetters: \"c\"",
                    LogLevel.Information,
                    Times.Once());

                _loggerMock.VerifyLogging(
                    $"Mapped SolveParameters:\n{result}",
                    LogLevel.Information,
                    Times.Once());
            }
        }

        [Test]
        public void MapToSolveParameters_NullRequest_ReturnsDefaultSolveParameters()
        {
            SolveParameters result = _requestMapper.MapToSolveParameters(null!);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.Empty);
                Assert.That(result.MisplacedLetters, Is.Empty);
                Assert.That(result.ExcludeLetters, Is.Empty);
                _loggerMock.VerifyLogging(
                    "Received null Request, returning default SolveParameters",
                    LogLevel.Warning,
                    Times.Once());
            }
        }

        [Test]
        public void MapToSolveParameters_RequestWithShortStrings_PadsWithQuestionMarks()
        {
            var request = new Request
            {
                CorrectLetters = "a",
                MisplacedLetters = string.Empty,
                ExcludedLetters = string.Empty,
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
                Assert.That(result.MisplacedLetters, Is.EqualTo(['?', '?', '?', '?', '?']));
                Assert.That(result.ExcludeLetters, Is.Empty);
            }
        }

        [Test]
        public void MapToSolveParameters_RequestWithLongerThanFiveChars_CutsShortToFive()
        {
            var request = new Request
            {
                CorrectLetters = new string('a', 6),
                MisplacedLetters = string.Empty,
                ExcludedLetters = string.Empty,
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.EqualTo(['a', 'a', 'a', 'a', 'a']));
                Assert.That(result.MisplacedLetters, Is.EqualTo(['?', '?', '?', '?', '?']));
                Assert.That(result.ExcludeLetters, Is.Empty);
            }
        }

        [Test]
        public void MapToSolveParameters_RequestWithUpperChar_ReplacedWithLower()
        {
            var request = new Request
            {
                CorrectLetters = "A",
                MisplacedLetters = string.Empty,
                ExcludedLetters = string.Empty,
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
                Assert.That(result.MisplacedLetters, Is.EqualTo(['?', '?', '?', '?', '?']));
                Assert.That(result.ExcludeLetters, Is.Empty);
            }
        }

        [Test]
        public void MapToSolveParameters_RequestWithNonLetterCharacters_ReplacedWithQuestionMark()
        {
            var request = new Request
            {
                CorrectLetters = "a$",
                MisplacedLetters = "b!",
                ExcludedLetters = "c",
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
                Assert.That(result.MisplacedLetters, Is.EqualTo(['b', '?', '?', '?', '?']));
                Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));
            }
        }
    }
}