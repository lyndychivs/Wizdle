namespace Wizdle.Tests.Solver
{
    using System;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Repository;
    using Wizdle.Solver;
    using Wizdle.Validator;

    [TestFixture]
    public class WordSolverConstructorTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly Mock<IWordRepository> _wordRepositoryMock;

        private readonly Mock<ISolveParametersValidator> _solveParametersValidatorMock;

        public WordSolverConstructorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _wordRepositoryMock = new Mock<IWordRepository>();
            _solveParametersValidatorMock = new Mock<ISolveParametersValidator>();
        }

        [Test]
        public void Constructor_WhenWordRepositoryReturnsWords_ReturnsWordSolver()
        {
            _wordRepositoryMock.Setup(r => r.GetWords()).Returns(["a"]);

            var result = new WordSolver(
                _loggerMock.Object,
                _wordRepositoryMock.Object,
                _solveParametersValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
            _loggerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
        {
            ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(
                () =>
                new WordSolver(
                    null!,
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
                    _loggerMock.Object,
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
                    _loggerMock.Object,
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
                _loggerMock.Object,
                _wordRepositoryMock.Object,
                _solveParametersValidatorMock.Object);

            Assert.That(result, Is.Not.Null);
            _loggerMock.VerifyLogging("No Words returned from IWordRepository", LogLevel.Error, Times.Once());
        }
    }
}