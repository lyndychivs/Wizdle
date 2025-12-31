namespace Wizdle.Unit.Tests;

using System;

using Microsoft.Extensions.Logging;

using Moq;

internal static class ILoggerExtensions
{
    internal static void VerifyLogging(this Mock<ILogger> logger, string expectedMessage, LogLevel expectedLogLevel, Times times)
    {
        Func<object, Type, bool> state = (v, t) => v.ToString().CompareTo(expectedMessage, StringComparison.OrdinalIgnoreCase) == 0;

        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            times);
    }
}
