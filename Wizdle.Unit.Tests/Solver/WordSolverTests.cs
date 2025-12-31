namespace Wizdle.Unit.Tests.Solver;

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
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(false);
        _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);

        // Act
        IEnumerable<string> result = _wordSolver.Solve(new SolveParameters());

        // Assert
        Assert.That(result, Is.Empty);
        _loggerMock.VerifyLogging(
            "SolveParameters is not valid, returning empty",
            LogLevel.Warning,
            Times.Once());
    }

    [Test]
    public void Solve_NoWordsReturnedFromWordRepository_ReturnsDefaultResponse()
    {
        // Arrange
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns([]);
        _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);

        // Act
        IEnumerable<string> result = _wordSolver.Solve(new SolveParameters());

        // Assert
        Assert.That(result, Is.Empty);
        _loggerMock.VerifyLogging(
            "No Words returned from IWordRepository, returning empty",
            LogLevel.Error,
            Times.Once());
    }

    [Test]
    public void Solve_ExcludeLettersFiltersOutWordsWithLetter_ReturnsFiltersWords()
    {
        // Arrange
        List<string> words = ["hates", "round", "climb"];
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
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
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
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
    public void Solve_KnownCorrectAndMisplacedLetters_ReturnsFiltersWords()
    {
        // Arrange
        List<string> words = ["hates", "hater", "hated"];
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
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

    [Test]
    public void Solve_EmptyCorrectLettersAndMisplacedLetters_ReturnsAllWords()
    {
        // Arrange
        List<string> words = ["a", "b"];
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
        _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
        var parameters = new SolveParameters
        {
            ExcludeLetters = [],
            CorrectLetters = [],
            MisplacedLetters = [],
        };

        // Act
        IEnumerable<string> result = _wordSolver.Solve(parameters);

        // Assert
        Assert.That(result, Is.EqualTo(words));
    }

    [Test]
    public void Solve_CorrectLetterUnknownButMisplacedLetterKnown_ReturnsFilteredWords()
    {
        // Arrange
        List<string> words = ["hates", "hater"];
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
        _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
        var parameters = new SolveParameters
        {
            ExcludeLetters = [],
            CorrectLetters = ['?', '?', '?', '?', '?'],
            MisplacedLetters = ['s', '?', '?', '?', '?'],
        };

        // Act
        IEnumerable<string> result = _wordSolver.Solve(parameters);

        // Assert
        Assert.That(result, Is.EqualTo(["hates"]));
    }

    [Test]
    public void Solve_OnlyMisplacedLetterKnown_ReturnsFilteredWords()
    {
        // Arrange
        List<string> words = ["snoop", "spoon"];
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
        _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
        var parameters = new SolveParameters
        {
            ExcludeLetters = [],
            CorrectLetters = ['?', '?', '?', '?', '?'],
            MisplacedLetters = ['?', '?', '?', '?', 'p'],
        };

        // Act
        IEnumerable<string> result = _wordSolver.Solve(parameters);

        // Assert
        Assert.That(result, Is.EqualTo(["spoon"]));
    }

    [Test]
    public void Solve_OnlyCorrectLetterKnown_ReturnsFilterWords()
    {
        // Arrange
        List<string> words = ["hates", "hater", "hated"];
        _solveParametersValidatorMock.Setup(v => v.IsValid(It.IsAny<SolveParameters>())).Returns(true);
        _wordRepositoryMock.Setup(r => r.GetWords()).Returns(words);
        _wordSolver = new WordSolver(_loggerMock.Object, _wordRepositoryMock.Object, _solveParametersValidatorMock.Object);
        var parameters = new SolveParameters
        {
            ExcludeLetters = [],
            CorrectLetters = ['?', '?', '?', '?', 'r'],
            MisplacedLetters = ['?', '?', '?', '?', '?'],
        };

        // Act
        IEnumerable<string> result = _wordSolver.Solve(parameters);

        // Assert
        Assert.That(result, Is.EqualTo(["hater"]));
    }
}
