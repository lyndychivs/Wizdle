﻿namespace Wizdle.PerformanceTests
{
    using BenchmarkDotNet.Attributes;

    using Microsoft.Extensions.Logging;

    using Serilog;
    using Serilog.Extensions.Logging;

    using Wizdle.Models;

    using ILogger = Microsoft.Extensions.Logging.ILogger;

    public class WizdleEngineTests
    {
        [Params(1, 2, 3, 4, 5)]
        #pragma warning disable SA1401 // Field should be made private
        public int WordLength;
        #pragma warning restore SA1401

        private const string WordSource = "zonal";

        private const string WordSourceReverse = "la.oz";

        private readonly WizdleEngine _wizdleEngine;

        private string? _word;

        private string? _wordReverse;

        public WizdleEngineTests()
        {
            _wizdleEngine = new WizdleEngine(CreateConsoleLogger());
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            _word = WordSource[..WordLength];
            _wordReverse = WordSourceReverse[..WordLength];
        }

        [Benchmark]
        public void WizdleEngine_WithOnlyCorrectLetters()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = _word!,
                MisplacedLetters = string.Empty,
                ExcludeLetters = string.Empty,
            };

            _ = _wizdleEngine.ProcessWizdleRequest(request);
        }

        [Benchmark]
        public void WizdleEngine_WithOnlyMisplacedLetters()
        {
            var request = new WizdleRequest
            {
                CorrectLetters = string.Empty,
                MisplacedLetters = _wordReverse!,
                ExcludeLetters = string.Empty,
            };

            _ = _wizdleEngine.ProcessWizdleRequest(request);
        }

        private static ILogger CreateConsoleLogger()
        {
            return new SerilogLoggerFactory(
                new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .CreateLogger()).CreateLogger<WizdleEngine>();
        }
    }
}