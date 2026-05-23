namespace Wizdle.Api.Functional.Tests;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using NUnit.Framework;

using Wizdle.Api.Functional.Tests.Setup;
using Wizdle.Models;

[TestFixture]
public class RateLimitWindowResetTests
{
    private const string RequestUri = "/";

    private const int PermitLimit = 2;
    private const int WindowSeconds = 5;

    private readonly HttpClient _httpClient;

    private readonly ContainerHandle _containerHandle;

    public RateLimitWindowResetTests()
    {
        _containerHandle = new WizdleApiContainerBuilder()
            .WithPermitLimit(PermitLimit)
            .WithWindowSeconds(WindowSeconds)
            .BuildAsync()
            .GetAwaiter()
            .GetResult();

        _httpClient = new HttpClient()
        {
            BaseAddress = _containerHandle.Url,
        };
    }

    [Test]
    public async Task PostWizdle_AfterRateLimitWindow_AllowsNewRequests()
    {
        // Arrange
        var request = new WizdleRequest
        {
            CorrectLetters = "s?ort",
            MisplacedLetters = "?????",
            ExcludeLetters = "haebukin",
        };

        // Act & Assert
        // Fill up the rate limit
        for (int i = 0; i < PermitLimit; i++)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        // Verify rate limit is exceeded
        using (HttpResponseMessage rateLimitedResponse = await _httpClient.PostAsJsonAsync(RequestUri, request))
        {
            Assert.That(rateLimitedResponse.StatusCode, Is.EqualTo(HttpStatusCode.TooManyRequests));
        }

        // Wait for the rate limit window to reset
        await Task.Delay(TimeSpan.FromSeconds(WindowSeconds + 1));

        // Verify we can make requests again
        using HttpResponseMessage newWindowResponse = await _httpClient.PostAsJsonAsync(RequestUri, request);
        Assert.That(
            newWindowResponse.StatusCode,
            Is.EqualTo(HttpStatusCode.OK),
            "Request should succeed after rate limit window resets");
    }

    [OneTimeTearDown]
    public async ValueTask Dispose()
    {
        _httpClient.Dispose();
        await _containerHandle.DisposeAsync();
    }
}
