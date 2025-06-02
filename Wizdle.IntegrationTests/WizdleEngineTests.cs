namespace Wizdle.IntegrationTests
{
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
        public void WizdleEngine_TrySolveWordle_Request1()
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
        public void WizdleEngine_TrySolveWordle_Request2()
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
        public void WizdleEngine_TrySolveWordle_Request3()
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
        public void WizdleEngine_TrySolveWordle_Request4()
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

        private ILogger CreateConsoleLogger()
        {
            return new SerilogLoggerFactory(
                new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger()).CreateLogger(nameof(WizdleEngine));
        }
    }
}