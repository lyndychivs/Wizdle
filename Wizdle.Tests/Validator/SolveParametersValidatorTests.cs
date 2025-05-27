namespace Wizdle.Tests.Validator
{
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
        public void IsValid_ValidParameters_ReturnsValid()
        {
            var solveParameters = new SolveParameters
            {
                CorrectLetters = ['?', '?', '?', '?', '?'],
                MisplacedLetters = ['a', 'b', 'c', 'd', 'e'],
                ExcludeLetters = ['x', 'y', 'z'],
            };

            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(solveParameters);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.True);
                Assert.That(validatorResponse.Errors, Is.Empty);
                _loggerMock.VerifyNoOtherCalls();
            }
        }

        [Test]
        public void IsValid_NullSolveParameters_ReturnsInvalid()
        {
            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(null!);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Is.EqualTo(["SolveParameters is null"]));
                _loggerMock.VerifyLogging("SolveParameters is null", LogLevel.Debug, Times.Once());
            }
        }

        [TestCase(4)]
        [TestCase(6)]
        public void IsValid_CorrectLettersCountNotFive_ReturnsInvalid(int lettersLength)
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
            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(solveParameters);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("SolveParameters.CorrectLetters Letter count is not equal to 5"));
                _loggerMock.VerifyLogging("SolveParameters.CorrectLetters Letter count is not equal to 5", LogLevel.Debug, Times.Once());
            }
        }

        [TestCase(4)]
        [TestCase(6)]
        public void IsValid_MisplacedLettersCountNotFive_ReturnsInvalid(int lettersLength)
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
            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(solveParameters);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("SolveParameters.MisplacedLetters Letter count is not equal to 5"));
                _loggerMock.VerifyLogging("SolveParameters.MisplacedLetters Letter count is not equal to 5", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_CorrectAndMisplacedSameLetterAtIndex_ReturnsInvalid()
        {
            var solveParameters = new SolveParameters
            {
                CorrectLetters = ['a', '?', '?', '?', '?'],
                MisplacedLetters = ['a', '?', '?', '?', '?'],
                ExcludeLetters = [],
            };

            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(solveParameters);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Has.Some.Contains("CorrectLetters and MisplacedLetters contain the same letter at index 0, Letter: 'a'"));
                _loggerMock.VerifyLogging("CorrectLetters and MisplacedLetters contain the same letter at index 0, Letter: 'a'", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_ExcludeLetterExistsInCorrectLetters_ReturnsInvalid()
        {
            var solveParameters = new SolveParameters
            {
                CorrectLetters = ['a', 'b', 'c', 'd', 'e'],
                MisplacedLetters = ['f', 'g', 'h', 'i', 'j'],
                ExcludeLetters = ['a'],
            };

            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(solveParameters);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: 'a'"));
                _loggerMock.VerifyLogging("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: 'a'", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_ExcludeLetterExistsInMisplacedLetters_ReturnsInvalid()
        {
            var solveParameters = new SolveParameters
            {
                CorrectLetters = ['a', 'b', 'c', 'd', 'e'],
                MisplacedLetters = ['f', 'g', 'h', 'i', 'j'],
                ExcludeLetters = ['f'],
            };

            ValidatorResponse validatorResponse = _solveParametersValidator.IsValid(solveParameters);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: 'f'"));
                _loggerMock.VerifyLogging("ExcludeLetters contains a letter that exists in CorrectLetters or MisplacedLetters, Letter: 'f'", LogLevel.Debug, Times.Once());
            }
        }
    }
}