namespace Wizdle.Api.Unit.Tests;

using System;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

[TestFixture]
public class CustomExceptionHandlerConstructorTests
{
    [Test]
    public void Constructor_WhenLoggerIsNull_ThrowsArgumentNullException()
    {
        Mock<ILogger<CustomExceptionHandler>> loggerMock = new(MockBehavior.Strict);

        Assert.Throws<ArgumentNullException>(() => new CustomExceptionHandler(null!));
    }

    [Test]
    public void Constructor_WhenLoggerIsProvided_DoesNotThrow()
    {
        Mock<ILogger<CustomExceptionHandler>> loggerMock = new(MockBehavior.Strict);

        Assert.DoesNotThrow(() => new CustomExceptionHandler(loggerMock.Object));
    }
}
