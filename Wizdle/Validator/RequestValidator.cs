namespace Wizdle.Validator
{
    using System;

    using Microsoft.Extensions.Logging;

    using Wizdle.Models;

    internal class RequestValidator : IRequestValidator
    {
        private readonly ILogger _logger;

        internal RequestValidator(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ValidatorResponse IsValid(Request request)
        {
            var validatorResponse = new ValidatorResponse();

            if (request is null)
            {
                _logger.LogDebug($"Received null {nameof(Request)}");

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add("Request cannot be null");

                return validatorResponse;
            }

            if (request.CorrectLetters == null)
            {
                string message = $"{nameof(Request)}.{nameof(Request.CorrectLetters)} cannot be null";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
            }

            if (request.MisplacedLetters == null)
            {
                string message = $"{nameof(Request)}.{nameof(Request.MisplacedLetters)} cannot be null";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
            }

            if (request.ExcludedLetters == null)
            {
                string message = $"{nameof(Request)}.{nameof(Request.ExcludedLetters)} cannot be null";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
            }

            if (request.CorrectLetters?.Length > 5)
            {
                string message = $"{nameof(Request)}.{nameof(Request.CorrectLetters)} cannot be longer than 5 characters";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
            }

            if (request.MisplacedLetters?.Length > 5)
            {
                string message = $"{nameof(Request)}.{nameof(Request.MisplacedLetters)} cannot be longer than 5 characters";
                _logger.LogDebug(message);

                validatorResponse.IsValid = false;
                validatorResponse.Errors.Add(message);
            }

            return validatorResponse;
        }
    }
}