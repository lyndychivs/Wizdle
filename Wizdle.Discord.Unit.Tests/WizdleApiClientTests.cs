namespace Wizdle.Discord.Unit.Tests;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Wizdle.Discord;
using Wizdle.Models;

[TestFixture]
public class WizdleApiClientTests
{
    private static readonly string[] ExpectedWords = ["crane", "stare"];

    private static readonly string[] ExpectedMessages = ["No words found."];

    [Test]
    public async Task PostWizdleRequestAsync_WhenCalled_SendsPostRequestToRootPath()
    {
        HttpRequestMessage? capturedRequest = null;
        WizdleApiClient client = CreateClient(new WizdleResponse(), req => capturedRequest = req);

        await client.PostWizdleRequestAsync(new WizdleRequest());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(capturedRequest?.Method, Is.EqualTo(HttpMethod.Post));
            Assert.That(capturedRequest?.RequestUri?.AbsolutePath, Is.EqualTo("/"));
        }
    }

    [Test]
    public async Task PostWizdleRequestAsync_WhenResponseContainsWords_ReturnsWords()
    {
        WizdleResponse fakeResponse = new() { Words = ["crane", "stare"] };
        WizdleApiClient client = CreateClient(fakeResponse);

        WizdleResponse result = await client.PostWizdleRequestAsync(new WizdleRequest());

        Assert.That(result.Words, Is.EqualTo(ExpectedWords));
    }

    [Test]
    public async Task PostWizdleRequestAsync_WhenResponseContainsMessages_ReturnsMessages()
    {
        WizdleResponse fakeResponse = new()
        {
            Messages = ["No words found."],
        };
        WizdleApiClient client = CreateClient(fakeResponse);

        WizdleResponse result = await client.PostWizdleRequestAsync(new WizdleRequest());

        Assert.That(result.Messages, Is.EqualTo(ExpectedMessages));
    }

    [Test]
    public async Task PostWizdleRequestAsync_WhenResponseBodyIsNull_ReturnsEmptyWizdleResponse()
    {
        FakeHttpMessageHandler handler = new(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("null", Encoding.UTF8, "application/json"),
        });
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("http://localhost"),
        };
        WizdleApiClient client = new(httpClient);

        WizdleResponse result = await client.PostWizdleRequestAsync(new WizdleRequest());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Words, Is.Empty);
            Assert.That(result.Messages, Is.Empty);
        }
    }

    private static WizdleApiClient CreateClient(
        WizdleResponse response,
        Action<HttpRequestMessage>? onRequest = null)
    {
        FakeHttpMessageHandler handler = new(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(response),
            },
            onRequest);
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("http://localhost"),
        };

        return new WizdleApiClient(httpClient);
    }
}
