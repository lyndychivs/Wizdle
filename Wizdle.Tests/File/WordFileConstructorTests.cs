namespace Wizdle.Tests.File
{
    using System;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.File;

    [TestFixture]
    public class WordFileConstructorTests
    {
        [Test]
        public void Constructor_WithValidLogger_ReturnsWordFile()
        {
            var loggerMock = new Mock<ILogger>();

            var wordFile = new WordFile(loggerMock.Object);

            Assert.That(wordFile, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => new WordFile(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex?.ParamName, Is.EqualTo("logger"));
                Assert.That(ex?.Message, Does.Contain("Value cannot be null. (Parameter 'logger')"));
            }
        }
    }
}