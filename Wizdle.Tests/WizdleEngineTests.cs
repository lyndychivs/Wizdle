namespace Wizdle.Tests
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Mapper;
    using Wizdle.Models;
    using Wizdle.Solver;
    using Wizdle.Validator;

    [TestFixture]
    public class WizdleEngineTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly Mock<IRequestValidator> _requestValidatorMock;

        private readonly Mock<IRequestMapper> _requestMapperMock;

        private readonly Mock<IWordSolver> _wordSolver;

        private readonly WizdleEngine _wizdleEngine;

        public WizdleEngineTests()
        {
            _loggerMock = new Mock<ILogger>();
            _requestValidatorMock = new Mock<IRequestValidator>();
            _requestMapperMock = new Mock<IRequestMapper>();
            _wordSolver = new Mock<IWordSolver>();
            _wizdleEngine = new WizdleEngine(
                _loggerMock.Object,
                _requestValidatorMock.Object,
                _requestMapperMock.Object,
                _wordSolver.Object);
        }

        [Test]
        public void ProcessWizdleRequest_WhenRequestIsInvalid_ReturnsErrorResponse()
        {
            // Arrange
            var request = new WizdleRequest();
            List<string> errors = ["Invalid input"];
            _requestValidatorMock.Setup(v => v.IsValid(request)).Returns(new ValidatorResponse { IsValid = false, Errors = errors });

            // Act
            WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.Empty);
                Assert.That(response.Messages, Is.EqualTo(errors));
            }
        }

        [Test]
        public void ProcessWizdleRequest_WhenRequestIsValid_ReturnsResponseWithWords()
        {
            // Arrange
            var request = new WizdleRequest
            {
                CorrectLetters = "a....",
                MisplacedLetters = "l....",
                ExcludeLetters = "c",
            };

            var solveParams = new SolveParameters();
            List<string> words = ["apple", "angle"];

            _requestValidatorMock.Setup(v => v.IsValid(request)).Returns(new ValidatorResponse { IsValid = true, Errors = [] });
            _requestMapperMock.Setup(m => m.MapToSolveParameters(request)).Returns(solveParams);
            _wordSolver.Setup(s => s.Solve(solveParams)).Returns(words);

            // Act
            WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.EqualTo(words));
                Assert.That(response.Messages, Is.EqualTo(["Found 2 Word(s) matching the criteria."]));
                _loggerMock.VerifyLogging(
                    "Processing WizdleRequest: CorrectLetters: \"a....\"   MisplacedLetters: \"l....\" ExcludeLetters: \"c\"",
                    LogLevel.Information,
                    Times.Once());
                _loggerMock.VerifyLogging(
                    "Found 2          Word(s) matching the criteria.",
                    LogLevel.Information,
                    Times.Once());
            }
        }

        [Test]
        public void ProcessWizdleRequest_WhenNoWordsMatch_ReturnsEmptyWords()
        {
            // Arrange
            var request = new WizdleRequest
            {
                CorrectLetters = "X",
                MisplacedLetters = "Y",
                ExcludeLetters = "Z",
            };
            var solveParams = new SolveParameters();
            List<string> words = [];

            _requestValidatorMock.Setup(v => v.IsValid(request)).Returns(new ValidatorResponse { IsValid = true, Errors = [] });
            _requestMapperMock.Setup(m => m.MapToSolveParameters(request)).Returns(solveParams);
            _wordSolver.Setup(s => s.Solve(solveParams)).Returns(words);

            // Act
            WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.Empty);
                Assert.That(response.Messages, Is.EqualTo(["Found 0 Word(s) matching the criteria."]));
                _loggerMock.VerifyLogging(
                    "Processing WizdleRequest: CorrectLetters: \"X\"       MisplacedLetters: \"Y\"     ExcludeLetters: \"Z\"",
                    LogLevel.Information,
                    Times.Once());
                _loggerMock.VerifyLogging(
                    "Found 0          Word(s) matching the criteria.",
                    LogLevel.Information,
                    Times.Once());
            }
        }
    }
}