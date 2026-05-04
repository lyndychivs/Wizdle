namespace Wizdle.Discord.Unit.Tests;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal sealed class FakeHttpMessageHandler(
    HttpResponseMessage response,
    Action<HttpRequestMessage>? onRequest = null)
    : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        onRequest?.Invoke(request);
        return Task.FromResult(response);
    }
}
