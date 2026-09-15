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
        HttpResponseMessage secondResponse = await client.PostAsJsonAsync(RequestUri, ValidRequest);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.TooManyRequests));
        }
    }
}
