namespace Wizdle
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using Wizdle.Mapper;
    using Wizdle.Models;
    using Wizdle.Solver;
    using Wizdle.Validator;

    /// <summary>
    /// The engine for processing requests to the Wizdle library.
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
        /// Processes the given <see cref="WizdleRequest"/> and returns a <see cref="WizdleResponse"/>.
        /// </summary>
        /// <param name="request">The request containing criteria for selecting words during the Solve.</param>
        /// <returns>A response containing the matching words and a message.</returns>
        public WizdleResponse ProcessWizdleRequest(WizdleRequest request)
        {
            ValidatorResponse validatorResponse = _requestValidator.IsValid(request);
            if (validatorResponse.IsValid == false)
            {
                return new WizdleResponse()
                {
                    Words = [],
                    Message = validatorResponse.Errors,
                };
            }

            _logger.LogInformation(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-25} {1,-25} {2,-25} {3}",
                    $"Processing {nameof(WizdleRequest)}:",
                    $"{nameof(request.CorrectLetters)}: \"{request.CorrectLetters}\"",
                    $"{nameof(request.MisplacedLetters)}: \"{request.MisplacedLetters}\"",
                    $"{nameof(request.ExcludeLetters)}: \"{request.ExcludeLetters}\""));

            SolveParameters solveParameters = _requestMapper.MapToSolveParameters(request);

            IEnumerable<string> words = _wordSolver.Solve(solveParameters);

            _logger.LogInformation(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0,-5} {1,-10} {2}",
                    $"Found",
                    words.Count(),
                    "Word(s) matching the criteria."));

            return new WizdleResponse()
            {
                Words = words,
                Message = [$"Found {words.Count()} Word(s) matching the criteria."],
            };
        }
    }
}