namespace Wizdle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using Wizdle.Mapper;
    using Wizdle.Models;
    using Wizdle.Solver;
    using Wizdle.Validator;

    /// <summary>
    /// The main engine for processing requests in the Wizdle application.
    /// </summary>
    public class WizdleEngine
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
            : this(logger, new RequestValidator(logger), new RequestMapper(logger), new WordSolver(logger))
        {
        }

        internal WizdleEngine(ILogger logger, IRequestValidator requestValidator, IRequestMapper requestMapper, IWordSolver solver)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _requestValidator = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
            _requestMapper = requestMapper ?? throw new ArgumentNullException(nameof(requestMapper));
            _wordSolver = solver ?? throw new ArgumentNullException(nameof(solver));
        }

        /// <summary>
        /// Processes the given <see cref="Request"/> and returns a <see cref="Response"/>.
        /// </summary>
        /// <param name="request">The request containing criteria for selecting words during the Solve.</param>
        /// <returns>A response containing the matching words and a message.</returns>
        public Response GetResponseForRequest(Request request)
        {
            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);
            if (validatorResponse.IsValid == false)
            {
                return new Response()
                {
                    Words = [],
                    Message = validatorResponse.Errors,
                };
            }

            _logger.LogInformation(
                $"Processing {nameof(Request)}:"
                + $"\n{nameof(request.CorrectLetters)}: \"{request.CorrectLetters}\""
                + $"\n{nameof(request.MisplacedLetters)}: \"{request.MisplacedLetters}\""
                + $"\n{nameof(request.ExcludedLetters)}: \"{request.ExcludedLetters}\"");

            SolveParameters solveParameters = _requestMapper.MapToSolveParameters(request);

            IEnumerable<string> words = _wordSolver.Solve(solveParameters);

            return new Response()
            {
                Words = words,
                Message = [$"Found {words.Count()} Word(s) matching the criteria."],
            };
        }
    }
}