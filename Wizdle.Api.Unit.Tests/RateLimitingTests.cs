namespace Wizdle.Api.Unit.Tests;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using NUnit.Framework;

using Wizdle.Models;

[TestFixture]
public class RateLimitingTests
{
    private const string RequestUri = "/";

    private static readonly WizdleRequest ValidRequest = new()
    {
        CorrectLetters = "s?ort",
        MisplacedLetters = "?????",
        ExcludeLetters = "haebukin",
    };

    private static readonly WizdleRequest AltRequest = new()
    {
        CorrectLetters = "?????",
        MisplacedLetters = "?????",
        ExcludeLetters = "z",
    };

    private WebApplicationFactory<Program> _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("RateLimiting:PermitLimit", "1");
                builder.UseSetting("RateLimiting:WindowSeconds", "60");
            });
    }

    [TearDown]
    public async Task TearDown()
    {
        await _factory.DisposeAsync();
    }

    [Test]
    public async Task PostWizdle_WithinRateLimit_ReturnsOk()
    {
        using HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync(RequestUri, ValidRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task PostWizdle_WhenRateLimitExceeded_ReturnsTooManyRequests()
    {
        using HttpClient client = _factory.CreateClient();

        HttpResponseMessage firstResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);
        HttpResponseMessage secondResponse = await client.PostAsJsonAsync(RequestUri, AltRequest);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.TooManyRequests));
        }
    }

    [Test]
    public async Task PostWizdle_WhenCachedResponseExists_DoesNotConsumeRateLimitPermit()
    {
        using HttpClient client = _factory.CreateClient();

        // First request consumes the single permit and populates the cache.
        HttpResponseMessage firstResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);

        // Second identical request should be served from cache, not consuming a permit.
        HttpResponseMessage secondResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);

        // Third request is a different key, so it bypasses the cache and hits the rate limiter,
        // which should now be exhausted.
        HttpResponseMessage thirdResponse = await client.PostAsJsonAsync(RequestUri, AltRequest);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(thirdResponse.StatusCode, Is.EqualTo(HttpStatusCode.TooManyRequests));
        }
    }
}
