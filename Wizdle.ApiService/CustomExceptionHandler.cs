namespace Wizdle.ApiService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    using static System.Net.Mime.MediaTypeNames;

    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"{nameof(CustomExceptionHandler)} caught:");

            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = Text.Plain;
            httpContext.Response.WriteAsync("An unexpected error occurred. Please try again later.", cancellationToken);

            return ValueTask.FromResult(true);
        }
    }
}