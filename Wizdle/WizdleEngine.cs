namespace Wizdle;

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Wizdle.Mapper;
using Wizdle.Models;
using Wizdle.Solver;
using Wizdle.Validator;

/// <summary>
/// The engine for processing requests to the Wizdle library.
/// </summary>
public sealed partial class WizdleEngine
{
    private readonly ILogger _logger;

    private readonly IRequestValidator _requestValidator;

    private readonly IWordSolver _wordSolver;

    private readonly IRequestMapper _requestMapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="WizdleEngine"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> interface to use.</param>
    public WizdleEngine(ILogger logger)
        : this(
              logger,
              new RequestValidator(logger),
              new RequestMapper(logger),
              new WordSolver(logger))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WizdleEngine"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> interface to use.</param>
    public WizdleEngine(ILogger<WizdleEngine> logger)
        : this(
              logger,
              new RequestValidator(logger),
              new RequestMapper(logger),
              new WordSolver(logger))
    {
    }

    internal WizdleEngine(
        ILogger logger,
        IRequestValidator requestValidator,
        IRequestMapper requestMapper,
        IWordSolver solver)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _requestValidator = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
        _requestMapper = requestMapper ?? throw new ArgumentNullException(nameof(requestMapper));
        _wordSolver = solver ?? throw new ArgumentNullException(nameof(solver));
    }

    /// <summary>
    /// Processes the given <see cref="WizdleRequest"/> and returns a <see cref="WizdleResponse"/>.
    /// </summary>
    /// <param name="request">The request containing criteria for selecting words during the Solve.</param>
    /// <returns>A response containing the matching words and a message.</returns>
    public WizdleResponse ProcessWizdleRequest(WizdleRequest request)
    {
        IEnumerable<string> requestErrors = _requestValidator.GetErrors(request);
        if (requestErrors.Any())
        {
            return new WizdleResponse()
            {
                Messages = requestErrors,
            };
        }

        LogProcessingRequest(
            _logger,
            nameof(WizdleRequest),
            request.CorrectLetters,
            request.MisplacedLetters,
            request.ExcludeLetters);

        SolveParameters solveParameters = _requestMapper.MapToSolveParameters(request);

        IEnumerable<string> words = _wordSolver.Solve(solveParameters);
        int wordCount = words.Count();

        LogFoundWords(_logger, wordCount);

        return new WizdleResponse()
        {
            Words = words,
            Messages = [$"Found {wordCount} Word(s) matching the criteria."],
        };
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Processing {RequestType}: CorrectLetters: \"{CorrectLetters}\", MisplacedLetters: \"{MisplacedLetters}\", ExcludeLetters: \"{ExcludeLetters}\"")]
    static partial void LogProcessingRequest(
        ILogger logger,
        string requestType,
        string correctLetters,
        string misplacedLetters,
        string excludeLetters);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Found {WordCount} Word(s) matching the criteria.")]
    static partial void LogFoundWords(ILogger logger, int wordCount);
}
