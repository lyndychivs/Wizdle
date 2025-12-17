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
public class PostWizdleTests
{
    private const string RequestUri = "/";

    private readonly HttpClient _httpClient;
    private readonly string _testUrl;

    public PostWizdleTests()
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
    public async Task PostWizdle_WithValidContent_ReturnsSuccess()
    {
        // Arrange
        var request = new WizdleRequest
        {
            CorrectLetters = "s?ort",
            MisplacedLetters = "?????",
            ExcludeLetters = "haebukin",
        };

        var expectedResponse = new WizdleResponse
        {
            Messages = ["Found 1 Word(s) matching the criteria."],
            Words = ["sport"],
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
            Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null!)]
    public async Task PostWizdle_WithInvalidContent_ReturnsBadRequest(string content)
    {
        // Arrange & Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, content);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(await response.Content.ReadAsStringAsync(), Is.EqualTo(string.Empty));
        }
    }

    [Test]
    public async Task PostWizdle_WithNullCorrectLetters_ReturnsMessageWithWarning()
    {
        // Arrange
        var request = new WizdleRequest
        {
            CorrectLetters = null!,
        };

        var expectedResponse = new WizdleResponse
        {
            Messages = ["WizdleRequest.CorrectLetters cannot be null"],
            Words = [],
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
            Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
        }
    }

    [Test]
    public async Task PostWizdle_WithNullMisplacedLetters_ReturnsMessageWithWarning()
    {
        // Arrange
        var request = new WizdleRequest
        {
            MisplacedLetters = null!,
        };

        var expectedResponse = new WizdleResponse
        {
            Messages = ["WizdleRequest.MisplacedLetters cannot be null"],
            Words = [],
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
            Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
        }
    }

    [Test]
    public async Task PostWizdle_WithNullExcludeLetters_ReturnsMessageWithWarning()
    {
        // Arrange
        var request = new WizdleRequest
        {
            ExcludeLetters = null!,
        };

        var expectedResponse = new WizdleResponse
        {
            Messages = ["WizdleRequest.ExcludeLetters cannot be null"],
            Words = [],
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
            Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task PostWizdle_WithEmptyCorrectLetters_ReturnsResponseWithWords(string letters)
    {
        // Arrange
        var request = new WizdleRequest
        {
            CorrectLetters = letters,
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(result?.Words, Is.Not.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task PostWizdle_WithEmptyMisplacedLetters_ReturnsResponseWithWords(string letters)
    {
        // Arrange
        var request = new WizdleRequest
        {
            MisplacedLetters = letters,
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(result?.Words, Is.Not.Empty);
        }
    }

    [TestCase("")]
    [TestCase(" ")]
    public async Task PostWizdle_WithEmptyExcludeLetters_ReturnsResponseWithWords(string letters)
    {
        // Arrange
        var request = new WizdleRequest
        {
            ExcludeLetters = letters,
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(result?.Words, Is.Not.Empty);
        }
    }

    [Test]
    public async Task PostWizdle_WithNonLetterInputs_ReturnsResponseWithWordsIgnoringNonLetterInputs()
    {
        // Arrange
        var request = new WizdleRequest
        {
            CorrectLetters = "1!",
            MisplacedLetters = "2£",
            ExcludeLetters = "3>",
        };

        // Act
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(RequestUri, request);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            WizdleResponse? result = await response.Content.ReadFromJsonAsync<WizdleResponse>();

            Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
            Assert.That(result?.Words, Is.Not.Empty);
        }
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
