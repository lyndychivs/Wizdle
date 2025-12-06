namespace Wizdle.Validator;

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

    public ValidatorResponse IsValid(WizdleRequest request)
    {
        var validatorResponse = new ValidatorResponse();

        if (request is null)
        {
            _logger.LogDebug($"Received null {nameof(WizdleRequest)}");

            validatorResponse.IsValid = false;
            validatorResponse.Errors.Add("WizdleRequest cannot be null");

            return validatorResponse;
        }

        if (request.CorrectLetters == null)
        {
            string message = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.CorrectLetters)} cannot be null";
            _logger.LogDebug(message);

            validatorResponse.IsValid = false;
            validatorResponse.Errors.Add(message);
        }

        if (request.MisplacedLetters == null)
        {
            string message = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.MisplacedLetters)} cannot be null";
            _logger.LogDebug(message);

            validatorResponse.IsValid = false;
            validatorResponse.Errors.Add(message);
        }

        if (request.ExcludeLetters == null)
        {
            string message = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.ExcludeLetters)} cannot be null";
            _logger.LogDebug(message);

            validatorResponse.IsValid = false;
            validatorResponse.Errors.Add(message);
        }

        if (request.CorrectLetters?.Length > 5)
        {
            string message = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.CorrectLetters)} cannot be longer than 5 characters";
            _logger.LogDebug(message);

            validatorResponse.IsValid = false;
            validatorResponse.Errors.Add(message);
        }

        if (request.MisplacedLetters?.Length > 5)
        {
            string message = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.MisplacedLetters)} cannot be longer than 5 characters";
            _logger.LogDebug(message);

            validatorResponse.IsValid = false;
            validatorResponse.Errors.Add(message);
        }

        return validatorResponse;
    }
}
