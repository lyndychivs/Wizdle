namespace Wizdle.Discord.Unit.Tests;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

internal sealed class ThrowingHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromException<HttpResponseMessage>(new HttpRequestException("Simulated failure"));
}
