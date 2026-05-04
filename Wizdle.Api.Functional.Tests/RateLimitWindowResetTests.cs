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

    private readonly HttpClient _httpClient;
    private readonly string _testUrl;

    public RateLimitWindowResetTests()
    {
        _httpClient = new HttpClient();

        _testUrl = ContainerSetup.GetWizdleApiUrl().Result;
        if (string.IsNullOrWhiteSpace(_testUrl))
        {
            throw new InvalidOperationException("Failed to obtain Wizdle API URL from container setup.");
        }

        _httpClient.BaseAddress = new Uri(_testUrl);
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

        const int permitLimit = 60;
        const int windowSeconds = 60;

        // Act & Assert
        // Fill up the rate limit
        for (int i = 0; i < permitLimit; i++)
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
        await Task.Delay(TimeSpan.FromSeconds(windowSeconds + 1));

        // Verify we can make requests again
        using HttpResponseMessage newWindowResponse = await _httpClient.PostAsJsonAsync(RequestUri, request);
        Assert.That(
            newWindowResponse.StatusCode,
            Is.EqualTo(HttpStatusCode.OK),
            "Request should succeed after rate limit window resets");
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
