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
            var request = new WizdleRequest
            {
                CorrectLetters = "....a",
                MisplacedLetters = "b....",
                ExcludeLetters = "c",
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
                Assert.That(validatorResponse.Errors, Has.Exactly(1).EqualTo("WizdleRequest cannot be null"));
                _loggerMock.VerifyLogging("Received null WizdleRequest", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_NullCorrectLetters_ReturnsInvalidWithError()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = null!,
                MisplacedLetters = "a",
                ExcludeLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Is.EqualTo(["WizdleRequest.CorrectLetters cannot be null"]));
                _loggerMock.VerifyLogging("WizdleRequest.CorrectLetters cannot be null", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_NullMisplacedLetters_ReturnsInvalidWithError()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = "a",
                MisplacedLetters = null!,
                ExcludeLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Is.EqualTo(["WizdleRequest.MisplacedLetters cannot be null"]));
                _loggerMock.VerifyLogging("WizdleRequest.MisplacedLetters cannot be null", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_NullExcludedLetters_ReturnsInvalidWithError()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = "a",
                MisplacedLetters = "b",
                ExcludeLetters = null!,
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Is.EqualTo(["WizdleRequest.ExcludeLetters cannot be null"]));
                _loggerMock.VerifyLogging("WizdleRequest.ExcludeLetters cannot be null", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_CorrectLettersTooLong_ReturnsInvalidWithError()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = "abcdef",
                MisplacedLetters = "a",
                ExcludeLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Is.EqualTo(["WizdleRequest.CorrectLetters cannot be longer than 5 characters"]));
                _loggerMock.VerifyLogging("WizdleRequest.CorrectLetters cannot be longer than 5 characters", LogLevel.Debug, Times.Once());
            }
        }

        [Test]
        public void IsValid_MisplacedLettersTooLong_ReturnsInvalidWithError()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = "a",
                MisplacedLetters = "abcdef",
                ExcludeLetters = "b",
            };

            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);

            using (Assert.EnterMultipleScope())
            {
                Assert.That(validatorResponse.IsValid, Is.False);
                Assert.That(validatorResponse.Errors, Is.EqualTo(["WizdleRequest.MisplacedLetters cannot be longer than 5 characters"]));
                _loggerMock.VerifyLogging("WizdleRequest.MisplacedLetters cannot be longer than 5 characters", LogLevel.Debug, Times.Once());
            }
        }
    }
}