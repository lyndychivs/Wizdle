namespace Wizdle.Unit.Tests.Mapper;

using System;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using Wizdle.Mapper;

[TestFixture]
public class RequestMapperConstructor
{
    [Test]
    public void Constructor_WithValidLogger_ReturnsRequestMapper()
    {
        var loggerMock = new Mock<ILogger>();

        var result = new RequestMapper(loggerMock.Object);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        ArgumentNullException? argumentNullException = Assert.Throws<ArgumentNullException>(() => new RequestMapper(null!));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(argumentNullException?.ParamName, Is.EqualTo("logger"));
            Assert.That(argumentNullException?.Message, Does.StartWith("Value cannot be null. (Parameter 'logger')"));
        }
    }
}
