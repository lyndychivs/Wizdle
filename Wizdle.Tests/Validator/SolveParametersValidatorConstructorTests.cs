namespace Wizdle.Tests.Validator
{
    using System;

    using Microsoft.Extensions.Logging;

    using Moq;

    using NUnit.Framework;

    using Wizdle.Validator;

    [TestFixture]
    public class SolveParametersValidatorConstructorTests
    {
        [Test]
        public void Constructor_WithValidLogger_ReturnsSolveParametersValidator()
        {
            var loggerMock = new Mock<ILogger>();

            var result = new SolveParametersValidator(loggerMock.Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => new SolveParametersValidator(null!));

            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex?.ParamName, Is.EqualTo("logger"));
                Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'logger')"));
            }
        }
    }
}