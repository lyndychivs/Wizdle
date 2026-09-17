namespace Wizdle.Api;

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Handles unhandled exceptions raised while processing a request.
/// </summary>
public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger{CustomExceptionHandler}"/> interface to use.</param>
    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Attempts to handle the given <paramref name="exception"/> by returning a generic error response.
    /// </summary>
    /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
    /// <param name="exception">The unhandled <see cref="Exception"/> that was thrown.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to cancel the operation with.</param>
    /// <returns>A task representing the asynchronous operation, containing <see langword="true"/> to indicate the exception was handled.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, $"{nameof(CustomExceptionHandler)} caught:");

        httpContext.Response.StatusCode = 500;
        httpContext.Response.ContentType = Text.Plain;
        await httpContext.Response.WriteAsync("An unexpected error occurred. Please try again later.", cancellationToken);

        return true;
    }
}
