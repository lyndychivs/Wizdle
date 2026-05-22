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

    private const int PermitLimit = 3;

    private readonly HttpClient _httpClient;

    private readonly string _testUrl;

    public RateLimitExceededTests()
    {
        _httpClient = new HttpClient();

        _testUrl = ContainerSetup.GetWizdleApiUrl(permitLimit: PermitLimit).Result;
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

        const int extraRequests = 5;

        // Act & Assert
        // Send requests up to the permit limit
        for (int i = 0; i < PermitLimit; i++)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.OK),
                $"Request {i + 1} should succeed (within rate limit)");
        }

        // Send requests beyond the limit
        for (int i = 0; i < extraRequests; i++)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);
            Assert.That(
                response.StatusCode,
                Is.EqualTo(HttpStatusCode.TooManyRequests),
                $"Request {PermitLimit + i + 1} should be rate limited");
        }
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
