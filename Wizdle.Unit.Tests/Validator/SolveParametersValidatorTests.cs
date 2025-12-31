namespace Wizdle.Unit.Tests.Validator;

using System.Linq;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using Wizdle.Solver;
using Wizdle.Validator;

[TestFixture]
public class SolveParametersValidatorTests
{
    private readonly Mock<ILogger> _loggerMock;

    private readonly SolveParametersValidator _solveParametersValidator;

    public SolveParametersValidatorTests()
    {
        _loggerMock = new Mock<ILogger>();
        _solveParametersValidator = new SolveParametersValidator(_loggerMock.Object);
    }

    [Test]
    public void IsValid_ValidParameters_ReturnsTrue()
    {
        var solveParameters = new SolveParameters
        {
            CorrectLetters = ['?', '?', '?', '?', '?'],
            MisplacedLetters = ['a', 'b', 'c', 'd', 'e'],
            ExcludeLetters = ['x', 'y', 'z'],
        };

        bool isValid = _solveParametersValidator.IsValid(solveParameters);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.True);
            _loggerMock.VerifyNoOtherCalls();
        }
    }

    [Test]
    public void IsValid_NullSolveParameters_ReturnsFalse()
    {
        bool isValid = _solveParametersValidator.IsValid(null!);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);
            _loggerMock.VerifyLogging("SolveParameters cannot be null", LogLevel.Debug, Times.Once());
        }
    }

    [TestCase(4)]
    [TestCase(6)]
    public void IsValid_CorrectLettersCountNotFive_ReturnsFalse(int lettersLength)
    {
        // Arrange
        var solveParameters = new SolveParameters
        {
            CorrectLetters = [],
            MisplacedLetters = ['b', 'b', 'b', 'b', 'b'],
            ExcludeLetters = [],
        };

        solveParameters.CorrectLetters = [.. Enumerable.Repeat('a', lettersLength)];

        // Act
        bool isValid = _solveParametersValidator.IsValid(solveParameters);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);
            _loggerMock.VerifyLogging("SolveParameters.CorrectLetters Letter count is not equal to 5", LogLevel.Debug, Times.Once());
        }
    }

    [TestCase(4)]
    [TestCase(6)]
    public void IsValid_MisplacedLettersCountNotFive_ReturnsFalse(int lettersLength)
    {
        // Arrange
        var solveParameters = new SolveParameters
        {
            CorrectLetters = ['a', 'a', 'a', 'a', 'a'],
            MisplacedLetters = [],
            ExcludeLetters = [],
        };

        solveParameters.MisplacedLetters = [.. Enumerable.Repeat('b', lettersLength)];

        // Act
        bool isValid = _solveParametersValidator.IsValid(solveParameters);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);
            _loggerMock.VerifyLogging("SolveParameters.MisplacedLetters Letter count is not equal to 5", LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void IsValid_CorrectAndMisplacedSameLetterAtIndex_ReturnsFalse()
    {
        var solveParameters = new SolveParameters
        {
            CorrectLetters = ['a', '?', '?', '?', '?'],
            MisplacedLetters = ['a', '?', '?', '?', '?'],
            ExcludeLetters = [],
        };

        bool isValid = _solveParametersValidator.IsValid(solveParameters);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);
            _loggerMock.VerifyLogging("CorrectLetters and MisplacedLetters contain the same letter at index 0, Letter: 'a'", LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void IsValid_ExcludeLetterExistsInCorrectLetters_ReturnsFalse()
    {
        var solveParameters = new SolveParameters
        {
            CorrectLetters = ['a', 'b', 'c', 'd', 'e'],
            MisplacedLetters = ['f', 'g', 'h', 'i', 'j'],
            ExcludeLetters = ['a'],
        };

        bool isValid = _solveParametersValidator.IsValid(solveParameters);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);
            _loggerMock.VerifyLogging("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: 'a'", LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void IsValid_ExcludeLetterExistsInMisplacedLetters_ReturnsFalse()
    {
        var solveParameters = new SolveParameters
        {
            CorrectLetters = ['a', 'b', 'c', 'd', 'e'],
            MisplacedLetters = ['f', 'g', 'h', 'i', 'j'],
            ExcludeLetters = ['f'],
        };

        bool isValid = _solveParametersValidator.IsValid(solveParameters);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);
            _loggerMock.VerifyLogging("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: 'f'", LogLevel.Debug, Times.Once());
        }
    }
}
