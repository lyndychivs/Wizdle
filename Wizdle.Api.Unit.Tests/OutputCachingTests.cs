namespace Wizdle.Api.Unit.Tests;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using Wizdle.Models;

[TestFixture]
public class OutputCachingTests
{
    private const string RequestUri = "/";

    private static readonly WizdleRequest ValidRequest = new()
    {
        CorrectLetters = "s?ort",
        MisplacedLetters = "?????",
        ExcludeLetters = "haebukin",
    };

    private WebApplicationFactory<Program> _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _factory.DisposeAsync();
    }

    [Test]
    public async Task PostWizdle_WhenCalledTwiceWithSameRequest_SecondResponseIsFromCache()
    {
        using HttpClient client = _factory.CreateClient();

        HttpResponseMessage firstResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);
        HttpResponseMessage secondResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(firstResponse.Headers.Contains("Age"), Is.False);
            Assert.That(secondResponse.Headers.Contains("Age"), Is.True);
        }
    }

    [Test]
    public async Task PostWizdle_WhenCalledWithDifferentRequests_BothReturnOk()
    {
        using HttpClient client = _factory.CreateClient();

        WizdleRequest differentRequest = new()
        {
            CorrectLetters = "crane",
            MisplacedLetters = "?????",
            ExcludeLetters = "xyz",
        };

        HttpResponseMessage firstResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);
        HttpResponseMessage secondResponse = await client.PostAsJsonAsync(RequestUri, differentRequest);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(firstResponse.Headers.Contains("Age"), Is.False);
            Assert.That(secondResponse.Headers.Contains("Age"), Is.False);
        }
    }

    [Test]
    public async Task PostWizdle_WhenCalledTwiceWithErrorRequest_SecondResponseIsNotFromCache()
    {
        using HttpClient client = _factory.CreateClient();

        HttpResponseMessage firstResponse = await client.PostAsJsonAsync<WizdleRequest?>(RequestUri, null!);
        HttpResponseMessage secondResponse = await client.PostAsJsonAsync<WizdleRequest?>(RequestUri, null!);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstResponse.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK));
            Assert.That(firstResponse.Headers.Contains("Age"), Is.False);
            Assert.That(secondResponse.Headers.Contains("Age"), Is.False);
        }
    }
}
