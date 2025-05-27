namespace Wizdle.Tests
{
    using System;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Models;

    [TestFixture]
    public class Test
    {
        [Test]
        public void Test1()
        {
            var logger = new Mock<ILogger>();
            var wizdleEngine = new WizdleEngine(logger.Object);
            var request = new Request
            {
                CorrectLetters = ".",
                MisplacedLetters = "..t.s",
                ExcludedLetters = "hae",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Console.WriteLine(response.Words.Count());
            Console.WriteLine(string.Join(Environment.NewLine, response.Words));
        }

        [Test]
        public void Test2()
        {
            var logger = new Mock<ILogger>();
            var wizdleEngine = new WizdleEngine(logger.Object);
            var request = new Request
            {
                CorrectLetters = "s",
                MisplacedLetters = ".tt.s",
                ExcludedLetters = "haeudy",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Console.WriteLine(response.Words.Count());
            Console.WriteLine(string.Join(Environment.NewLine, response.Words));
        }

        [Test]
        public void Test3()
        {
            var logger = new Mock<ILogger>();
            var wizdleEngine = new WizdleEngine(logger.Object);
            var request = new Request
            {
                CorrectLetters = "s.o.t",
                MisplacedLetters = "....s",
                ExcludedLetters = "haeudyml",
            };

            Response response = wizdleEngine.GetResponseForRequest(request);

            Console.WriteLine(response.Words.Count());
            Console.WriteLine(string.Join(Environment.NewLine, response.Words));
        }
    }
}