namespace Wizdle.Console;

using System;

using CommandLine;

using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Extensions.Logging;

using Wizdle.Models;

using ILogger = Microsoft.Extensions.Logging.ILogger;

internal static partial class Program
{
    internal static void Main(string[] args)
    {
        TrySetTitle();

        Parser.Default.ParseArguments<SolveArguments>(args).WithParsed(CallWizdleEngine);
    }

    private static void CallWizdleEngine(SolveArguments solveArguments)
    {
        ILogger logger = CreateConsoleLogger();
        var wizdleEngine = new WizdleEngine(logger);
        var wizdleRequest = new WizdleRequest
        {
            CorrectLetters = solveArguments.CorrectLetters,
            MisplacedLetters = solveArguments.MisplacedLetters,
            ExcludeLetters = solveArguments.ExcludeLetters,
        };

        WizdleResponse response = wizdleEngine.ProcessWizdleRequest(wizdleRequest);

        string messages = string.Join(Environment.NewLine, response.Messages);
        if (string.IsNullOrWhiteSpace(messages) is false)
        {
            LogMessages(logger, messages);
        }

        string words = string.Join(Environment.NewLine, response.Words);
        if (string.IsNullOrWhiteSpace(words) is false)
        {
            LogWords(logger, words);
        }
    }

    private static ILogger CreateConsoleLogger()
    {
        return new SerilogLoggerFactory(
            new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(outputTemplate: "{Message}{NewLine}{Exception}").CreateLogger())
            .CreateLogger(nameof(WizdleEngine));
    }

    private static void TrySetTitle()
    {
        try
        {
            Console.Title = nameof(Wizdle);
        }
        catch
        {
        }
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "{Messages}")]
    static partial void LogMessages(ILogger logger, string messages);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "{Words}")]
    static partial void LogWords(ILogger logger, string words);
}
