namespace Wizdle.Unit.Tests.Validator;

using System.Collections.Generic;

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
    public void GetErrors_ValidRequest_ReturnsEmpty()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "....a",
            MisplacedLetters = "b....",
            ExcludeLetters = "c",
        };

        IEnumerable<string> errors = _requestValidator.GetErrors(request);

        Assert.That(errors, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Is.Empty);
            _loggerMock.VerifyNoOtherCalls();
        }
    }

    [Test]
    public void GetErrors_NullRequest_ReturnsError()
    {
        string expectedError = "WizdleRequest cannot be null";

        IEnumerable<string> errors = _requestValidator.GetErrors(null!);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Has.Exactly(1).EqualTo(expectedError));
            _loggerMock.VerifyLogging(expectedError, LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void GetErrors_NullCorrectLetters_ReturnsError()
    {
        string expectedError = "WizdleRequest.CorrectLetters cannot be null";
        var request = new WizdleRequest
        {
            CorrectLetters = null!,
            MisplacedLetters = "a",
            ExcludeLetters = "b",
        };

        IEnumerable<string> errors = _requestValidator.GetErrors(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Has.Exactly(1).EqualTo(expectedError));
            _loggerMock.VerifyLogging(expectedError, LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void GetErrors_NullMisplacedLetters_ReturnsError()
    {
        string expectedError = "WizdleRequest.MisplacedLetters cannot be null";
        var request = new WizdleRequest
        {
            CorrectLetters = "a",
            MisplacedLetters = null!,
            ExcludeLetters = "b",
        };

        IEnumerable<string> errors = _requestValidator.GetErrors(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Has.Exactly(1).EqualTo(expectedError));
            _loggerMock.VerifyLogging(expectedError, LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void GetErrors_NullExcludedLetters_ReturnsError()
    {
        string expectedError = "WizdleRequest.ExcludeLetters cannot be null";
        var request = new WizdleRequest
        {
            CorrectLetters = "a",
            MisplacedLetters = "b",
            ExcludeLetters = null!,
        };

        IEnumerable<string> errors = _requestValidator.GetErrors(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Has.Exactly(1).EqualTo(expectedError));
            _loggerMock.VerifyLogging(expectedError, LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void GetErrors_CorrectLettersTooLong_ReturnsError()
    {
        string expectedError = "WizdleRequest.CorrectLetters cannot be longer than 5 characters";
        var request = new WizdleRequest
        {
            CorrectLetters = "abcdef",
            MisplacedLetters = "a",
            ExcludeLetters = "b",
        };

        IEnumerable<string> errors = _requestValidator.GetErrors(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Has.Exactly(1).EqualTo(expectedError));
            _loggerMock.VerifyLogging(expectedError, LogLevel.Debug, Times.Once());
        }
    }

    [Test]
    public void GetErrors_MisplacedLettersTooLong_ReturnsError()
    {
        string expectedError = "WizdleRequest.MisplacedLetters cannot be longer than 5 characters";
        var request = new WizdleRequest
        {
            CorrectLetters = "a",
            MisplacedLetters = "abcdef",
            ExcludeLetters = "b",
        };

        IEnumerable<string> errors = _requestValidator.GetErrors(request);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(errors, Has.Exactly(1).EqualTo(expectedError));
            _loggerMock.VerifyLogging(expectedError, LogLevel.Debug, Times.Once());
        }
    }
}
