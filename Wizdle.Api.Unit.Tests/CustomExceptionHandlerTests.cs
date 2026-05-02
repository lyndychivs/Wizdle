namespace Wizdle.Api.Unit.Tests;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;

using NUnit.Framework;

using static System.Net.Mime.MediaTypeNames;

[TestFixture]
public class CustomExceptionHandlerTests
{
    private readonly FakeLogger<CustomExceptionHandler> _logger;

    private readonly CustomExceptionHandler _handler;

    public CustomExceptionHandlerTests()
    {
        _logger = new FakeLogger<CustomExceptionHandler>();
        _handler = new CustomExceptionHandler(_logger);
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionIsThrown_ReturnsTrue()
    {
        DefaultHttpContext httpContext = CreateHttpContext();

        bool result = await _handler.TryHandleAsync(httpContext, new InvalidOperationException("test"), CancellationToken.None);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionIsThrown_SetsStatusCode500()
    {
        DefaultHttpContext httpContext = CreateHttpContext();

        await _handler.TryHandleAsync(httpContext, new InvalidOperationException("test"), CancellationToken.None);

        Assert.That(httpContext.Response.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionIsThrown_SetsContentTypeToTextPlain()
    {
        DefaultHttpContext httpContext = CreateHttpContext();

        await _handler.TryHandleAsync(httpContext, new InvalidOperationException("test"), CancellationToken.None);

        Assert.That(httpContext.Response.ContentType, Is.EqualTo(Text.Plain));
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionIsThrown_WritesExpectedBody()
    {
        DefaultHttpContext httpContext = CreateHttpContext();
        MemoryStream body = (MemoryStream)httpContext.Response.Body;

        await _handler.TryHandleAsync(httpContext, new InvalidOperationException("test"), CancellationToken.None);

        body.Seek(0, SeekOrigin.Begin);
        using StreamReader reader = new(body);
        string content = await reader.ReadToEndAsync(TestContext.CurrentContext.CancellationToken);

        Assert.That(content, Is.EqualTo("An unexpected error occurred. Please try again later."));
    }

    [Test]
    public async Task TryHandleAsync_WhenExceptionIsThrown_LogsErrorWithException()
    {
        DefaultHttpContext httpContext = CreateHttpContext();
        InvalidOperationException exception = new("boom");

        await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        FakeLogRecord log = _logger.Collector.GetSnapshot().Single();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(log.Level, Is.EqualTo(LogLevel.Error));
            Assert.That(log.Exception, Is.SameAs(exception));
        }
    }

    private static DefaultHttpContext CreateHttpContext()
    {
        DefaultHttpContext httpContext = new();
        httpContext.Response.Body = new MemoryStream();
        return httpContext;
    }
}
