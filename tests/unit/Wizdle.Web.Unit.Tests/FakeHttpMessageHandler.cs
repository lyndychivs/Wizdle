namespace Wizdle.Web.Unit.Tests;

using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Wizdle.Models;

internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly WizdleResponse _response;

    private int _requestCount;

    internal FakeHttpMessageHandler(WizdleResponse response)
    {
        _response = response;
    }

    internal int RequestCount => _requestCount;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Interlocked.Increment(ref _requestCount);

        string json = JsonSerializer.Serialize(_response);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = content,
        });
    }
}
