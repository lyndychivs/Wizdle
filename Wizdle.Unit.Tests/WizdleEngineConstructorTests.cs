namespace Wizdle.Unit.Tests;

using System;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using Wizdle.Mapper;
using Wizdle.Solver;
using Wizdle.Validator;

[TestFixture]
public class WizdleEngineConstructorTests
{
    private readonly Mock<ILogger> _loggerMock;

    private readonly Mock<IRequestValidator> _requestValidatorMock;

    private readonly Mock<IRequestMapper> _requestMapperMock;

    private readonly Mock<IWordSolver> _wordSolverMock;

    public WizdleEngineConstructorTests()
    {
        _loggerMock = new Mock<ILogger>();
        _requestValidatorMock = new Mock<IRequestValidator>();
        _requestMapperMock = new Mock<IRequestMapper>();
        _wordSolverMock = new Mock<IWordSolver>();
    }

    [Test]
    public void Constructor_WithValidParameters_ReturnsWizdleEngine()
    {
        var result = new WizdleEngine(
            _loggerMock.Object,
            _requestValidatorMock.Object,
            _requestMapperMock.Object,
            _wordSolverMock.Object);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() =>
            new WizdleEngine(
                null!,
                _requestValidatorMock.Object,
                _requestMapperMock.Object,
                _wordSolverMock.Object));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("logger"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'logger')"));
        }
    }

    [Test]
    public void Constructor_WithNullRequestValidator_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() =>
            new WizdleEngine(
                _loggerMock.Object,
                null!,
                _requestMapperMock.Object,
                _wordSolverMock.Object));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("requestValidator"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'requestValidator')"));
        }
    }

    [Test]
    public void Constructor_WithNullRequestMapper_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() =>
            new WizdleEngine(
                _loggerMock.Object,
                _requestValidatorMock.Object,
                null!,
                _wordSolverMock.Object));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("requestMapper"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'requestMapper')"));
        }
    }

    [Test]
    public void Constructor_WithNullWordSolver_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() =>
            new WizdleEngine(
                _loggerMock.Object,
                _requestValidatorMock.Object,
                _requestMapperMock.Object,
                null!));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("solver"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'solver')"));
        }
    }
}
