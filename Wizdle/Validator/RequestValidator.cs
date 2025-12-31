namespace Wizdle.Validator;

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Wizdle.Models;

internal class RequestValidator : IRequestValidator
{
    private readonly ILogger _logger;

    internal RequestValidator(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<string> GetErrors(WizdleRequest request)
    {
        var errorList = new List<string>();

        if (request is null)
        {
            string error = $"{nameof(WizdleRequest)} cannot be null";
            _logger.LogDebug(error);
            errorList.Add(error);

            return errorList;
        }

        if (request.CorrectLetters is null)
        {
            string error = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.CorrectLetters)} cannot be null";
            _logger.LogDebug(error);
            errorList.Add(error);
        }

        if (request.MisplacedLetters is null)
        {
            string error = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.MisplacedLetters)} cannot be null";
            _logger.LogDebug(error);
            errorList.Add(error);
        }

        if (request.ExcludeLetters is null)
        {
            string error = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.ExcludeLetters)} cannot be null";
            _logger.LogDebug(error);
            errorList.Add(error);
        }

        if (request.CorrectLetters?.Length > 5)
        {
            string error = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.CorrectLetters)} cannot be longer than 5 characters";
            _logger.LogDebug(error);
            errorList.Add(error);
        }

        if (request.MisplacedLetters?.Length > 5)
        {
            string error = $"{nameof(WizdleRequest)}.{nameof(WizdleRequest.MisplacedLetters)} cannot be longer than 5 characters";
            _logger.LogDebug(error);
            errorList.Add(error);
        }

        return errorList;
    }
}
