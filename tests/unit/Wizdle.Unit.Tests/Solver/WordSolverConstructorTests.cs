namespace Wizdle.Unit.Tests.Solver;

using System;
using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

using Moq;

using NUnit.Framework;

using Wizdle.Repository;
using Wizdle.Solver;
using Wizdle.Validator;

[TestFixture]
public class WordSolverConstructorTests
{
    private readonly FakeLogger<WordSolver> _logger;

    private readonly Mock<IWordRepository> _wordRepositoryMock;

    private readonly Mock<ISolveParametersValidator> _solveParametersValidatorMock;

    public WordSolverConstructorTests()
    {
        _logger = new FakeLogger<WordSolver>();
        _wordRepositoryMock = new Mock<IWordRepository>(MockBehavior.Strict);
        _solveParametersValidatorMock = new Mock<ISolveParametersValidator>(MockBehavior.Strict);
    }

    [Test]
    public void Constructor_WhenWordRepositoryReturnsWords_ReturnsWordSolver()
    {
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(["a"]);

        var result = new WordSolver(
            _logger,
            _wordRepositoryMock.Object,
            _solveParametersValidatorMock.Object);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(_logger.Collector.GetSnapshot(), Is.Empty);
        }
    }

    [Test]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(
            () =>
            new WordSolver(
                (ILogger)null!,
                _wordRepositoryMock.Object,
                _solveParametersValidatorMock.Object));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("logger"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'logger')"));
        }
    }

    [Test]
    public void Constructor_WhenWordRepositoryIsNull_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(
            () =>
            new WordSolver(
                _logger,
                null!,
                _solveParametersValidatorMock.Object));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("wordRepository"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'wordRepository')"));
        }
    }

    [Test]
    public void Constructor_WhenSolveParametersValidatorIsNull_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(
            () =>
            new WordSolver(
                _logger,
                _wordRepositoryMock.Object,
                null!));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("wordParameterValidator"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'wordParameterValidator')"));
        }
    }

    [Test]
    public void Constructor_WhenWordRepositoryReturnsEmpty_LogsError()
    {
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns([]);

        var result = new WordSolver(
            _logger,
            _wordRepositoryMock.Object,
            _solveParametersValidatorMock.Object);

        Assert.That(result, Is.Not.Null);

        var logs = _logger.Collector.GetSnapshot();
        var errorLog = logs.Single(e => e.Level == LogLevel.Error);
        Assert.That(errorLog.Message, Does.Contain("No Words returned from IWordRepository"));
    }
}
