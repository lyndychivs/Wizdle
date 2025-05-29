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
            var request = new WizdleRequest
            {
                CorrectLetters = "a....",
                MisplacedLetters = "b....",
                ExcludeLetters = "c",
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
                Assert.That(result.MisplacedLetters, Is.EqualTo(['b', '?', '?', '?', '?']));
                Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));

                _loggerMock.VerifyLogging(
                    "Mapping WizdleRequest:    CorrectLetters: \"a....\"   MisplacedLetters: \"b....\" ExcludeLetters: \"c\"",
                    LogLevel.Information,
                    Times.Once());

                _loggerMock.VerifyLogging(
                    $"Mapped SolveParameters:   CorrectLetters: \"a????\"   MisplacedLetters: \"b????\" ExcludeLetters: \"c\"",
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
                    "Received null WizdleRequest, returning default SolveParameters",
                    LogLevel.Error,
                    Times.Once());
            }
        }

        [Test]
        public void MapToSolveParameters_RequestWithShortStrings_PadsWithQuestionMarks()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = "a",
                MisplacedLetters = string.Empty,
                ExcludeLetters = string.Empty,
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
            var request = new WizdleRequest
            {
                CorrectLetters = new string('a', 6),
                MisplacedLetters = string.Empty,
                ExcludeLetters = string.Empty,
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
            var request = new WizdleRequest
            {
                CorrectLetters = "A",
                MisplacedLetters = "B",
                ExcludeLetters = "C",
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

        [Test]
        public void MapToSolveParameters_RequestWithNonLetterCharacters_ReplacedWithQuestionMark()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = "a$",
                MisplacedLetters = "b!",
                ExcludeLetters = "c",
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

        [Test]
        public void MapToSolveParameters_RequestWithNonLetterCharactersInExcludeLetters_ExcludedFromSolveParameters()
        {
            var request = new WizdleRequest
            {
                ExcludeLetters = "c$%.!",
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));
        }

        [Test]
        public void MapToSolveParameters_RequestWithMultipleCharactersInExcludeLettters_OnlyIncludesOneInstanceOfEach()
        {
            var request = new WizdleRequest
            {
                ExcludeLetters = "abcabc",
            };

            SolveParameters result = _requestMapper.MapToSolveParameters(request);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ExcludeLetters, Is.EqualTo(['a', 'b', 'c']));
        }
    }
}