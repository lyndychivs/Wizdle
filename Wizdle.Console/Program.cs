namespace Wizdle.Console
{
    using System;
    using System.Linq;

    using CommandLine;

    using Microsoft.Extensions.Logging;

    using Serilog;
    using Serilog.Extensions.Logging;

    using Wizdle.Models;

    using ILogger = Microsoft.Extensions.Logging.ILogger;

    internal class Program
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

            if (response.Messages.Any())
            {
                logger.LogInformation(string.Join(Environment.NewLine, response.Messages));
            }

            if (response.Words.Any())
            {
                logger.LogInformation(string.Join(Environment.NewLine, response.Words));
            }
        }

        private static ILogger CreateConsoleLogger()
        {
            return new SerilogLoggerFactory(
                new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "{Message}{NewLine}{Exception}")
                .CreateLogger()).CreateLogger(nameof(WizdleEngine));
        }

        private static void TrySetTitle()
        {
            try
            {
                System.Console.Title = nameof(Wizdle);
            }
            catch
            {
            }
        }
    }
}