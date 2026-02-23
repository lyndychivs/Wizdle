namespace Wizdle.Validator;

using System;
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Wizdle.Models;

internal sealed partial class RequestValidator : IRequestValidator
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
            errorList.Add($"{nameof(WizdleRequest)} cannot be null");
            LogRequestNull(_logger, nameof(WizdleRequest));

            return errorList;
        }

        if (request.CorrectLetters is null)
        {
            errorList.Add($"{nameof(WizdleRequest)} {nameof(WizdleRequest.CorrectLetters)} cannot be null");
            LogPropertyNull(_logger, nameof(WizdleRequest), nameof(WizdleRequest.CorrectLetters));
        }

        if (request.MisplacedLetters is null)
        {
            errorList.Add($"{nameof(WizdleRequest)} {nameof(WizdleRequest.MisplacedLetters)} cannot be null");
            LogPropertyNull(_logger, nameof(WizdleRequest), nameof(WizdleRequest.MisplacedLetters));
        }

        if (request.ExcludeLetters is null)
        {
            errorList.Add($"{nameof(WizdleRequest)} {nameof(WizdleRequest.ExcludeLetters)} cannot be null");
            LogPropertyNull(_logger, nameof(WizdleRequest), nameof(WizdleRequest.ExcludeLetters));
        }

        if (request.CorrectLetters?.Length > 5)
        {
            errorList.Add($"{nameof(WizdleRequest)} {nameof(WizdleRequest.CorrectLetters)} cannot be longer than 5 characters");
            LogPropertyTooLong(_logger, nameof(WizdleRequest), nameof(WizdleRequest.CorrectLetters));
        }

        if (request.MisplacedLetters?.Length > 5)
        {
            errorList.Add($"{nameof(WizdleRequest)} {nameof(WizdleRequest.MisplacedLetters)} cannot be longer than 5 characters");
            LogPropertyTooLong(_logger, nameof(WizdleRequest), nameof(WizdleRequest.MisplacedLetters));
        }

        return errorList;
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "{RequestType} cannot be null")]
    static partial void LogRequestNull(
        ILogger logger,
        string requestType);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "{RequestType} {PropertyName} cannot be null")]
    static partial void LogPropertyNull(
        ILogger logger,
        string requestType,
        string propertyName);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "{RequestType} {PropertyName} cannot be longer than 5 characters")]
    static partial void LogPropertyTooLong(
        ILogger logger,
        string requestType,
        string propertyName);
}
