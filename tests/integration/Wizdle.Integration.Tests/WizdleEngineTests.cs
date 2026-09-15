namespace Wizdle.Integration.Tests;

using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Wizdle.Models;

using ILogger = Microsoft.Extensions.Logging.ILogger;

[TestFixture]
public partial class WizdleEngineTests
{
    private readonly ILogger _logger;

    private readonly WizdleEngine _wizdleEngine;

    public WizdleEngineTests()
    {
        _logger = Logger.CreateConsoleLogger<WizdleEngineTests>();
        _wizdleEngine = new WizdleEngine(_logger);
    }

    [Test]
    public void WizdleEngine_ValidRequest_ReturnsResponseWithWords()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = ".....",
            MisplacedLetters = "..t.s",
            ExcludeLetters = "hae",
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.Not.Empty);
        }

        LogMessages(_logger, response.Messages);
        LogWords(_logger, response.Words);
    }

    [Test]
    public void WizdleEngine_ValidRequestBasedOffRequestOneResponse_ReturnsResponseWithWords()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "....t",
            MisplacedLetters = "..rs.",
            ExcludeLetters = "haebu",
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.SupersetOf(["skirt", "snort", "sport"]));
        }

        LogMessages(_logger, response.Messages);
        LogWords(_logger, response.Words);
    }

    [Test]
    public void WizdleEngine_ValidRequestBasedOffRequestTwoResponse_ReturnsResponseWithWords()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "s..rt",
            MisplacedLetters = ".....",
            ExcludeLetters = "haebuki",
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.SupersetOf(["snort", "sport"]));
        }

        LogMessages(_logger, response.Messages);
        LogWords(_logger, response.Words);
    }

    [Test]
    public void WizdleEngine_ValidRequestBasedOffRequestThreeResponse_ReturnsResponseWithWords()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "s.ort",
            MisplacedLetters = ".....",
            ExcludeLetters = "haebukin",
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.SupersetOf(["sport"]));
        }

        LogMessages(_logger, response.Messages);
        LogWords(_logger, response.Words);
    }

    [Test]
    public void WizdleEngine_NullCorrectLetters_ReturnsMessageWithWarning()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = null!,
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["WizdleRequest CorrectLetters cannot be null"]));
            Assert.That(response.Words, Is.Empty);
        }
    }

    [Test]
    public void WizdleEngine_NullMisplacedLetters_ReturnsMessageWithWarning()
    {
        var request = new WizdleRequest
        {
            MisplacedLetters = null!,
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["WizdleRequest MisplacedLetters cannot be null"]));
            Assert.That(response.Words, Is.Empty);
        }
    }

    [Test]
    public void WizdleEngine_NullExcludeLetters_ReturnsMessageWithWarning()
    {
        var request = new WizdleRequest
        {
            ExcludeLetters = null!,
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["WizdleRequest ExcludeLetters cannot be null"]));
            Assert.That(response.Words, Is.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public void WizdleEngine_EmptyCorrectLetters_ReturnsResponseWithWords(string letters)
    {
        var request = new WizdleRequest
        {
            CorrectLetters = letters!,
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public void WizdleEngine_EmptyMisplacedLetters_ReturnsResponseWithWords(string letters)
    {
        var request = new WizdleRequest
        {
            MisplacedLetters = letters!,
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public void WizdleEngine_EmptyExcludeLetters_ReturnsResponseWithWords(string letters)
    {
        var request = new WizdleRequest
        {
            ExcludeLetters = letters!,
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [Test]
    public void WizdleEngine_NonLetterInputs_ReturnsResponseWithWordsIgnoringNonLetterInputs()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "1!",
            MisplacedLetters = "2£",
            ExcludeLetters = "3>",
        };

        WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Has.Some.Contains("Word(s) matching the criteria."));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "{Messages}")]
    static partial void LogMessages(ILogger logger, IEnumerable<string> messages);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "{Words}")]
    static partial void LogWords(ILogger logger, IEnumerable<string> words);
}
