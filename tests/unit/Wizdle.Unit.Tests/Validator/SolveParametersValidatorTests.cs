namespace Wizdle.Unit.Tests.Validator;

using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

using NUnit.Framework;

using Wizdle.Solver;
using Wizdle.Validator;

[TestFixture]
public class SolveParametersValidatorTests
{
    private readonly FakeLogger<SolveParametersValidator> _logger;

    private readonly SolveParametersValidator _solveParametersValidator;

    public SolveParametersValidatorTests()
    {
        _logger = new FakeLogger<SolveParametersValidator>();
        _solveParametersValidator = new SolveParametersValidator(_logger);
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
            Assert.That(_logger.Collector.GetSnapshot(), Is.Empty);
        }
    }

    [Test]
    public void IsValid_NullSolveParameters_ReturnsFalse()
    {
        bool isValid = _solveParametersValidator.IsValid(null!);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(isValid, Is.False);

            var logs = _logger.Collector.GetSnapshot();
            var errorLogs = logs.Single(e => e.Level == LogLevel.Error);
            Assert.That(errorLogs.Message, Does.Contain("solveParameters cannot be null"));
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

            var logs = _logger.Collector.GetSnapshot();
            var debugLog = logs.Single(e => e.Level == LogLevel.Debug);
            Assert.That(debugLog.Message, Does.Contain("CorrectLetters Letter count is not equal to 5"));
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

            var logs = _logger.Collector.GetSnapshot();
            var debugLog = logs.Single(e => e.Level == LogLevel.Debug);
            Assert.That(debugLog.Message, Does.Contain("MisplacedLetters Letter count is not equal to 5"));
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

            var logs = _logger.Collector.GetSnapshot();
            var debugLog = logs.Single(e => e.Level == LogLevel.Debug);
            Assert.That(debugLog.Message, Does.Contain("CorrectLetters and MisplacedLetters contain the same letter at index 0"));
            Assert.That(debugLog.Message, Does.Contain("Letter: 'a'"));
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

            var logs = _logger.Collector.GetSnapshot();
            var debugLog = logs.Single(e => e.Level == LogLevel.Debug);
            Assert.That(debugLog.Message, Does.Contain("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters"));
            Assert.That(debugLog.Message, Does.Contain("Letter: 'a'"));
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

            var logs = _logger.Collector.GetSnapshot();
            var debugLog = logs.Single(e => e.Level == LogLevel.Debug);
            Assert.That(debugLog.Message, Does.Contain("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters"));
            Assert.That(debugLog.Message, Does.Contain("Letter: 'f'"));
        }
    }
}
