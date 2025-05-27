namespace Wizdle.IntegrationTests
{
    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Models;

    [TestFixture]
    public class WizdleEngineTests
    {
        private readonly Mock<ILogger> _loggerMock;

        public WizdleEngineTests()
        {
            _loggerMock = new Mock<ILogger>();
        }

        [Test]
        public void WizdleEngine_TrySolveWordle_Request1()
        {
            var wizdleEngine = new WizdleEngine(_loggerMock.Object);

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
            var wizdleEngine = new WizdleEngine(_loggerMock.Object);

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
            var wizdleEngine = new WizdleEngine(_loggerMock.Object);

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
            var wizdleEngine = new WizdleEngine(_loggerMock.Object);

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
    }
}