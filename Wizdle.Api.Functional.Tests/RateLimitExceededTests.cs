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
public class RateLimitExceededTests
{
    private const string RequestUri = "/";

    private readonly HttpClient _httpClient;
    private readonly string _testUrl;

    public RateLimitExceededTests()
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
    public async Task PostWizdle_WhenRateLimitExceeded_ReturnsTooManyRequests()
    {
        // Arrange
        var request = new WizdleRequest
        {
            CorrectLetters = "s?ort",
            MisplacedLetters = "?????",
            ExcludeLetters = "haebukin",
        };

        const int permitLimit = 60;
        const int extraRequests = 5;

        // Act & Assert
        // First, send requests up to the permit limit - all should succeed
        for (int i = 0; i < permitLimit; i++)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                $"Request {i + 1} should succeed (within rate limit)");
        }

        // Now send requests beyond the limit - these should be rate limited
        for (int i = 0; i < extraRequests; i++)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.TooManyRequests),
                $"Request {permitLimit + i + 1} should be rate limited");
        }
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
