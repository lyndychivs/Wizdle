namespace Wizdle.Functional.Tests.Logging;

using NUnit.Framework;

using Serilog;
using Serilog.Extensions.Logging;

using ILogger = Microsoft.Extensions.Logging.ILogger;

internal static class Logger
{
    public static ILogger CreateConsoleLogger<T>()
    {
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console();

        if (TestContext.Out is not null)
        {
            loggerConfig = loggerConfig.WriteTo.TextWriter(TestContext.Progress);
        }

        return new SerilogLoggerFactory(loggerConfig.CreateLogger())
            .CreateLogger(nameof(T));
    }
}
