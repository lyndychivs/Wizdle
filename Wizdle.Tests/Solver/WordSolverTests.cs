namespace Wizdle.Tests.Solver
{
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Repository;
    using Wizdle.Solver;
    using Wizdle.Validator;

    [TestFixture]
    public class WordSolverTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly Mock<IWordRepository> _wordRepositoryMock;

        private readonly Mock<ISolveParametersValidator> _solveParametersValidatorMock;

        private WordSolver? _wordSolver;

        public WordSolverTests()
        {
            _loggerMock = new Mock<ILogger>();
            _wordRepositoryMock = new Mock<IWordRepository>();
            _solveParametersValidatorMock = new Mock<ISolveParametersValidator>();
        }

        [Test]
        public void Solve_InvalidParameters_ReturnsDefaultResponse()
        {
            // Arrange
            _ = _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(new ValidatorResponse { IsValid = false });
            _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);

            // Act
            IEnumerable<string> result = _wordSolver.Solve(new SolveParameters());

            // Assert
            Assert.That(result, Is.EqualTo(["hates", "round", "climb"]));
            _loggerMock.VerifyLogging(
                "SolveParameters is not valid, returning _defaultResponse",
                LogLevel.Warning,
                Times.Once());
        }

        [Test]
        public void Solve_NoWordsReturnedFromWordRepository_ReturnsDefaultResponse()
        {
            // Arrange
            _ = _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(new ValidatorResponse { IsValid = true });
            _ = _wordRepositoryMock.Setup(r => r.GetWords()).Returns([]);
            _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);

            // Act
            IEnumerable<string> result = _wordSolver.Solve(new SolveParameters());

            // Assert
            Assert.That(result, Is.EqualTo(["hates", "round", "climb"]));
            _loggerMock.VerifyLogging(
                "No Words returned from IWordRepository, returning _defaultResponse",
                LogLevel.Error,
                Times.Once());
        }

        [Test]
        public void Solve_ExcludeLettersFiltersOutWordsWithLetter_ReturnsFiltersWords()
        {
            // Arrange
            List<string> words = ["hates", "round", "climb"];
            _ = _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(new ValidatorResponse { IsValid = true });
            _ = _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
            _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
            var parameters = new SolveParameters
            {
                ExcludeLetters = ['a'],
                CorrectLetters = ['?', '?', '?', '?', '?'],
                MisplacedLetters = ['?', '?', '?', '?', '?'],
            };

            // Act
            IEnumerable<string> result = _wordSolver.Solve(parameters);

            // Assert
            Assert.That(result, Is.EqualTo(["round", "climb"]));
        }

        [Test]
        public void Solve_NoWordsAfterExclude_ReturnsEmpty()
        {
            // Arrange
            List<string> words = ["hates", "round"];
            _ = _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(new ValidatorResponse { IsValid = true });
            _ = _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
            _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
            var parameters = new SolveParameters
            {
                ExcludeLetters = ['h', 'r'],
                CorrectLetters = ['?', '?', '?', '?', '?'],
                MisplacedLetters = ['?', '?', '?', '?', '?'],
            };

            // Act
            IEnumerable<string> result = _wordSolver.Solve(parameters);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Solve_CorrectAndMisplacedLetters_FiltersWords()
        {
            // Arrange
            List<string> words = ["hates", "hater", "hated"];
            _ = _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(new ValidatorResponse { IsValid = true });
            _ = _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
            _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
            var parameters = new SolveParameters
            {
                ExcludeLetters = [],
                CorrectLetters = ['h', '?', '?', '?', '?'],
                MisplacedLetters = ['s', '?', '?', '?', '?'],
            };

            // Act
            IEnumerable<string> result = _wordSolver.Solve(parameters);

            // Assert
            Assert.That(result, Is.EquivalentTo(["hates"]));
        }
    }
}