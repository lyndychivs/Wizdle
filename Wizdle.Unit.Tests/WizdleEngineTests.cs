namespace Wizdle.Unit.Tests;

using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

using Moq;

using NUnit.Framework;

using Wizdle.Mapper;
using Wizdle.Models;
using Wizdle.Solver;
using Wizdle.Validator;

[TestFixture]
public class WizdleEngineTests
{
    private readonly FakeLogger<WizdleEngine> _logger;

    private readonly Mock<IRequestValidator> _requestValidatorMock;

    private readonly Mock<IRequestMapper> _requestMapperMock;

    private readonly Mock<IWordSolver> _wordSolver;

    private readonly WizdleEngine _wizdleEngine;

    public WizdleEngineTests()
    {
        _logger = new FakeLogger<WizdleEngine>();
        _requestValidatorMock = new Mock<IRequestValidator>();
        _requestMapperMock = new Mock<IRequestMapper>();
        _wordSolver = new Mock<IWordSolver>();
        _wizdleEngine = new WizdleEngine(
            _logger,
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
        _requestValidatorMock.Setup(v => v.GetErrors(request)).Returns(errors);

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

        _requestValidatorMock.Setup(v => v.GetErrors(request)).Returns([]);
        _requestMapperMock.Setup(m => m.MapToSolveParameters(request)).Returns(solveParams);
        _wordSolver.Setup(s => s.Solve(solveParams)).Returns(words);

        // Act
        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Words, Is.EqualTo(words));
            Assert.That(response.Messages, Is.EqualTo(["Found 2 Word(s) matching the criteria."]));

            var logs = _logger.Collector.GetSnapshot();

            var processingLog = logs.Single(e => e.Id == 1 && e.Level == LogLevel.Information);
            Assert.That(processingLog.Message, Does.Contain("Processing WizdleRequest"));
            Assert.That(processingLog.Message, Does.Contain("CorrectLetters: \"a....\""));
            Assert.That(processingLog.Message, Does.Contain("MisplacedLetters: \"l....\""));
            Assert.That(processingLog.Message, Does.Contain("ExcludeLetters: \"c\""));

            var foundLog = logs.Single(e => e.Id == 2 && e.Level == LogLevel.Information);
            Assert.That(foundLog.Message, Does.Contain("Found 2 Word(s) matching the criteria"));
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

        _requestValidatorMock.Setup(v => v.GetErrors(request)).Returns([]);
        _requestMapperMock.Setup(m => m.MapToSolveParameters(request)).Returns(solveParams);
        _wordSolver.Setup(s => s.Solve(solveParams)).Returns(words);

        // Act
        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Words, Is.Empty);
            Assert.That(response.Messages, Is.EqualTo(["Found 0 Word(s) matching the criteria."]));

            var logs = _logger.Collector.GetSnapshot();

            var processingLog = logs.Single(e => e.Id == 1 && e.Level == LogLevel.Information);
            Assert.That(processingLog.Message, Does.Contain("Processing WizdleRequest"));
            Assert.That(processingLog.Message, Does.Contain("CorrectLetters: \"X\""));
            Assert.That(processingLog.Message, Does.Contain("MisplacedLetters: \"Y\""));
            Assert.That(processingLog.Message, Does.Contain("ExcludeLetters: \"Z\""));

            var foundLog = logs.Single(e => e.Id == 2 && e.Level == LogLevel.Information);
            Assert.That(foundLog.Message, Does.Contain("Found 0 Word(s) matching the criteria"));
        }
    }
}
