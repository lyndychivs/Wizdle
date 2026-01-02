namespace Wizdle.Integration.Tests;

using Serilog;
using Serilog.Extensions.Logging;

using ILogger = Microsoft.Extensions.Logging.ILogger;

internal static class Logger
{
    public static ILogger CreateConsoleLogger<T>()
    {
        return new SerilogLoggerFactory(
            new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger()).CreateLogger(nameof(T));
    }
}
