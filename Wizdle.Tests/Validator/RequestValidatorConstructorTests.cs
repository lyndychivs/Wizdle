namespace Wizdle.Tests.Validator;

using System;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using Wizdle.Validator;

[TestFixture]
public class RequestValidatorConstructorTests
{
    [Test]
    public void Constructor_WithValidLogger_ReturnsRequestValidator()
    {
        var logger = new Mock<ILogger>();

        var result = new RequestValidator(logger.Object);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => new RequestValidator(null!));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("logger"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'logger')"));
        }
    }
}