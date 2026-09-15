namespace Wizdle.Unit.Tests.Mapper;

using System.Linq;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

using NUnit.Framework;

using Wizdle.Mapper;
using Wizdle.Models;
using Wizdle.Solver;

[TestFixture]
public class RequestMapperTests
{
    private readonly FakeLogger<WizdleEngine> _logger;

    private readonly RequestMapper _requestMapper;

    public RequestMapperTests()
    {
        _logger = new FakeLogger<WizdleEngine>();
        _requestMapper = new RequestMapper(_logger);
    }

    [Test]
    public void MapToSolveParameters_ValidRequest_MapsCorrectly()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "a....",
            MisplacedLetters = "b....",
            ExcludeLetters = "c",
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
            Assert.That(result.MisplacedLetters, Is.EqualTo(['b', '?', '?', '?', '?']));
            Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));

            var logs = _logger.Collector.GetSnapshot();

            var mappingLog = logs.Single(e => e.Id == 2 && e.Level == LogLevel.Information);
            Assert.That(mappingLog.Message, Is.EqualTo("Mapping WizdleRequest: [CorrectLetters: \"a....\", MisplacedLetters: \"b....\", ExcludeLetters: \"c\"]"));

            var mappedLog = logs.Single(e => e.Id == 3 && e.Level == LogLevel.Information);
            Assert.That(mappedLog.Message, Is.EqualTo("Mapped SolveParameters: [CorrectLetters: \"a, ?, ?, ?, ?\", MisplacedLetters: \"b, ?, ?, ?, ?\", ExcludeLetters: \"c\"]"));
        }
    }

    [Test]
    public void MapToSolveParameters_NullRequest_ReturnsDefaultSolveParameters()
    {
        SolveParameters result = _requestMapper.MapToSolveParameters(null!);

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.Empty);
            Assert.That(result.MisplacedLetters, Is.Empty);
            Assert.That(result.ExcludeLetters, Is.Empty);

            var logs = _logger.Collector.GetSnapshot();
            var errorLog = logs.Single(e => e.Id == 1 && e.Level == LogLevel.Error);
            Assert.That(errorLog.Message, Does.Contain("Received null WizdleRequest"));
            Assert.That(errorLog.Message, Does.Contain("returning default SolveParameters"));
        }
    }

    [Test]
    public void MapToSolveParameters_RequestWithShortStrings_PadsWithQuestionMarks()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "a",
            MisplacedLetters = string.Empty,
            ExcludeLetters = string.Empty,
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
            Assert.That(result.MisplacedLetters, Is.EqualTo(['?', '?', '?', '?', '?']));
            Assert.That(result.ExcludeLetters, Is.Empty);
        }
    }

    [Test]
    public void MapToSolveParameters_RequestWithLongerThanFiveChars_CutsShortToFive()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = new string('a', 6),
            MisplacedLetters = string.Empty,
            ExcludeLetters = string.Empty,
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.EqualTo(['a', 'a', 'a', 'a', 'a']));
            Assert.That(result.MisplacedLetters, Is.EqualTo(['?', '?', '?', '?', '?']));
            Assert.That(result.ExcludeLetters, Is.Empty);
        }
    }

    [Test]
    public void MapToSolveParameters_RequestWithUpperChar_ReplacedWithLower()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "A",
            MisplacedLetters = "B",
            ExcludeLetters = "C",
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
            Assert.That(result.MisplacedLetters, Is.EqualTo(['b', '?', '?', '?', '?']));
            Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));
        }
    }

    [Test]
    public void MapToSolveParameters_RequestWithNonLetterCharacters_ReplacedWithQuestionMark()
    {
        var request = new WizdleRequest
        {
            CorrectLetters = "a$",
            MisplacedLetters = "b!",
            ExcludeLetters = "c",
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.EqualTo(['a', '?', '?', '?', '?']));
            Assert.That(result.MisplacedLetters, Is.EqualTo(['b', '?', '?', '?', '?']));
            Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));
        }
    }

    [Test]
    public void MapToSolveParameters_RequestWithNonLetterCharactersInExcludeLetters_ExcludedFromSolveParameters()
    {
        var request = new WizdleRequest
        {
            ExcludeLetters = "c$%.!",
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));
    }

    [Test]
    public void MapToSolveParameters_RequestWithMultipleCharactersInExcludeLettters_OnlyIncludesOneInstanceOfEach()
    {
        var request = new WizdleRequest
        {
            ExcludeLetters = "abcabc",
        };

        SolveParameters result = _requestMapper.MapToSolveParameters(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ExcludeLetters, Is.EqualTo(['a', 'b', 'c']));
    }
}
