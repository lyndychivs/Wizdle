namespace Wizdle.IntegrationTests
{
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

            var request = new Request
            {
                CorrectLetters = ".....",
                MisplacedLetters = "..t.s",
                ExcludedLetters = "hae",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Assert.That(response, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.Not.Empty);
                Assert.That(response.Message, Is.Not.Empty);
            }

            Console.WriteLine(string.Join(Environment.NewLine, response.Message));
            Console.WriteLine(string.Join(Environment.NewLine, response.Words));
        }

        [Test]
        public void WizdleEngine_TrySolveWordle_Request2()
        {
            var wizdleEngine = new WizdleEngine(_logger);

            var request = new Request
            {
                CorrectLetters = "....t",
                MisplacedLetters = "..rs.",
                ExcludedLetters = "haebu",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Assert.That(response, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.Not.Empty);
                Assert.That(response.Message, Is.Not.Empty);
            }

            Console.WriteLine(string.Join(Environment.NewLine, response.Message));
            Console.WriteLine(string.Join(Environment.NewLine, response.Words));
        }

        [Test]
        public void WizdleEngine_TrySolveWordle_Request3()
        {
            var wizdleEngine = new WizdleEngine(_logger);

            var request = new Request
            {
                CorrectLetters = "s..rt",
                MisplacedLetters = ".....",
                ExcludedLetters = "haebuki",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Assert.That(response, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.Not.Empty);
                Assert.That(response.Message, Is.Not.Empty);
            }

            Console.WriteLine(string.Join(Environment.NewLine, response.Message));
            Console.WriteLine(string.Join(Environment.NewLine, response.Words));
        }

        [Test]
        public void WizdleEngine_TrySolveWordle_Request4()
        {
            var wizdleEngine = new WizdleEngine(_logger);

            var request = new Request
            {
                CorrectLetters = "s.ort",
                MisplacedLetters = ".....",
                ExcludedLetters = "haebukin",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Assert.That(response, Is.Not.Null);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.Words, Is.Not.Empty);
                Assert.That(response.Message, Is.Not.Empty);
            }

            Console.WriteLine(string.Join(Environment.NewLine, response.Message));
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