namespace Wizdle.Integration.Tests;

using System;

using NUnit.Framework;

using Serilog;
using Serilog.Extensions.Logging;

using Wizdle.Models;

using ILogger = Microsoft.Extensions.Logging.ILogger;

[TestFixture]
public class WizdleEngineTests
{
    private readonly ILogger _logger;

    public WizdleEngineTests()
    {
        _logger = CreateConsoleLogger();
    }

    [Test]
    public void WizdleEngine_ValidRequestOne_ReturnsResponseWithWords()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = ".....",
            MisplacedLetters = "..t.s",
            ExcludeLetters = "hae",
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 65 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
            Assert.That(response.Words, Has.Exactly(65).Items);
        }

        Console.WriteLine(string.Join(Environment.NewLine, response.Messages));
        Console.WriteLine(string.Join(Environment.NewLine, response.Words));
    }

    [Test]
    public void WizdleEngine_ValidRequestTwo_ReturnsResponseWithWords()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = "....t",
            MisplacedLetters = "..rs.",
            ExcludeLetters = "haebu",
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 3 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
            Assert.That(response.Words, Is.EqualTo(["skirt", "snort", "sport"]));
        }

        Console.WriteLine(string.Join(Environment.NewLine, response.Messages));
        Console.WriteLine(string.Join(Environment.NewLine, response.Words));
    }

    [Test]
    public void WizdleEngine_ValidRequestThree_ReturnsResponseWithWords()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = "s..rt",
            MisplacedLetters = ".....",
            ExcludeLetters = "haebuki",
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 2 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
            Assert.That(response.Words, Is.EqualTo(["snort", "sport"]));
        }

        Console.WriteLine(string.Join(Environment.NewLine, response.Messages));
        Console.WriteLine(string.Join(Environment.NewLine, response.Words));
    }

    [Test]
    public void WizdleEngine_ValidRequestFour_ReturnsResponseWithWords()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = "s.ort",
            MisplacedLetters = ".....",
            ExcludeLetters = "haebukin",
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 1 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
            Assert.That(response.Words, Is.EqualTo(["sport"]));
        }

        Console.WriteLine(string.Join(Environment.NewLine, response.Messages));
        Console.WriteLine(string.Join(Environment.NewLine, response.Words));
    }

    [Test]
    public void WizdleEngine_NullCorrectLetters_ReturnsMessageWithWarning()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = null!,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["WizdleRequest.CorrectLetters cannot be null"]));
            Assert.That(response.Words, Is.Empty);
        }
    }

    [Test]
    public void WizdleEngine_NullMisplacedLetters_ReturnsMessageWithWarning()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            MisplacedLetters = null!,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["WizdleRequest.MisplacedLetters cannot be null"]));
            Assert.That(response.Words, Is.Empty);
        }
    }

    [Test]
    public void WizdleEngine_NullExcludeLetters_ReturnsMessageWithWarning()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            ExcludeLetters = null!,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["WizdleRequest.ExcludeLetters cannot be null"]));
            Assert.That(response.Words, Is.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public void WizdleEngine_EmptyCorrectLetters_ReturnsResponseWithWords(string letters)
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = letters!,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public void WizdleEngine_EmptyMisplacedLetters_ReturnsResponseWithWords(string letters)
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            MisplacedLetters = letters!,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public void WizdleEngine_EmptyExcludeLetters_ReturnsResponseWithWords(string letters)
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            ExcludeLetters = letters!,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    [Test]
    public void WizdleEngine_NonLetterInputs_ReturnsResponseWithWordsIgnoringNonLetterInputs()
    {
        var wizdleEngine = new WizdleEngine(_logger);

        var request = new WizdleRequest
        {
            CorrectLetters = "1!",
            MisplacedLetters = "2£",
            ExcludeLetters = "3>",
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);

        Assert.That(response, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(response.Words, Is.Not.Empty);
        }
    }

    private ILogger CreateConsoleLogger()
    {
        return new SerilogLoggerFactory(
            new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()).CreateLogger(nameof(WizdleEngine));
    }
}
