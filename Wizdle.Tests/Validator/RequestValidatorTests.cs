namespace Wizdle.Tests.Validator
{
    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Models;
    using Wizdle.Validator;

    [TestFixture]
    public class RequestValidatorTests
    {
        private readonly Mock<ILogger> _loggerMock;

        private readonly RequestValidator _requestValidator;

        public RequestValidatorTests()
        {
            _loggerMock = new Mock<ILogger>();
            _requestValidator = new RequestValidator(_loggerMock.Object);
        }

        [Test]
        public void IsValid_ValidRequest_ReturnsValid()
        {
            var request = new Request
            {
                CorrectLetters = "a",
                MisplacedLetters = "b",
                ExcludedLetters = "c",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            Assert.That(validatorResponse, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.True);
                Assert.That(validatorResponse.Errors, Is.Empty);
                _loggerMock.VerifyNoOtherCalls();
            }
        }

        [Test]
        public void IsValid_NullRequest_ReturnsInvalidWithError()
        {
            ValidatorResponse validatorResponse = _requestValidator.IsValid(null!);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Has.Exactly(1).EqualTo("Request cannot be null"));
                _loggerMock.VerifyLogging("Received null Request", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_NullCorrectLetters_ReturnsInvalidWithError()
        {
            var request = new Request
            {
                CorrectLetters = null!,
                MisplacedLetters = "a",
                ExcludedLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("Request.CorrectLetters cannot be null"));
                _loggerMock.VerifyLogging("Request.CorrectLetters cannot be null", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_NullMisplacedLetters_ReturnsInvalidWithError()
        {
            var request = new Request
            {
                CorrectLetters = "a",
                MisplacedLetters = null!,
                ExcludedLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("Request.MisplacedLetters cannot be null"));
                _loggerMock.VerifyLogging("Request.MisplacedLetters cannot be null", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_NullExcludedLetters_ReturnsInvalidWithError()
        {
            var request = new Request
            {
                CorrectLetters = "a",
                MisplacedLetters = "b",
                ExcludedLetters = null!,
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("Request.ExcludedLetters cannot be null"));
                _loggerMock.VerifyLogging("Request.ExcludedLetters cannot be null", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_CorrectLettersTooLong_ReturnsInvalidWithError()
        {
            var request = new Request
            {
                CorrectLetters = "abcdef",
                MisplacedLetters = "a",
                ExcludedLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("Request.CorrectLetters cannot be longer than 5 characters"));
                _loggerMock.VerifyLogging("Request.CorrectLetters cannot be longer than 5 characters", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_MisplacedLettersTooLong_ReturnsInvalidWithError()
        {
            var request = new Request
            {
                CorrectLetters = "a",
                MisplacedLetters = "abcdef",
                ExcludedLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Does.Contain("Request.MisplacedLetters cannot be longer than 5 characters"));
                _loggerMock.VerifyLogging("Request.MisplacedLetters cannot be longer than 5 characters", LogLevel.Debug, Times.Once());
            }
        }
    }
}