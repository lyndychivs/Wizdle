namespace Wizdle.ApiService.IntegrationTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Aspire.Hosting;
    using Aspire.Hosting.Testing;

    using NUnit.Framework;

    using Projects;

    using Wizdle.Models;

    [TestFixture]
    public class PostWizdleTests
    {
        [Test]
        public async Task PostWizdle_WithValidContent_ReturnsSuccess()
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

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
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
                Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null!)]
        public async Task PostWizdle_WithInvalidContent_ReturnsInternalServerError(string content)
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

            // Act
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/wizdle", content);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(response.Content.ReadAsStringAsync().Result, Is.EqualTo("An unexpected error occurred. Please try again later."));
            }
        }

        [Test]
        public async Task PostWizdle_WithNullCorrectLetters_ReturnsMessageWithWarning()
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

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
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
                Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
            }
        }

        [Test]
        public async Task PostWizdle_WithNullMisplacedLetters_ReturnsMessageWithWarning()
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

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
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
                Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
            }
        }

        [Test]
        public async Task PostWizdle_WithNullExcludeLetters_ReturnsMessageWithWarning()
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

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
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(expectedResponse.Messages));
                Assert.That(result?.Words, Is.EqualTo(expectedResponse.Words));
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        public async Task PostWizdle_WithEmptyCorrectLetters_ReturnsResponseWithWords(string letters)
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

            var request = new WizdleRequest
            {
                CorrectLetters = letters,
            };

            // Act
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
                Assert.That(result?.Words, Is.Not.Empty);
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        public async Task PostWizdle_WithEmptyMisplacedLetters_ReturnsResponseWithWords(string letters)
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

            var request = new WizdleRequest
            {
                MisplacedLetters = letters,
            };

            // Act
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
                Assert.That(result?.Words, Is.Not.Empty);
            }
        }

        [TestCase("")]
        [TestCase(" ")]
        public async Task PostWizdle_WithEmptyExcludeLetters_ReturnsResponseWithWords(string letters)
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

            var request = new WizdleRequest
            {
                ExcludeLetters = letters,
            };

            // Act
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
                Assert.That(result?.Words, Is.Not.Empty);
            }
        }

        [Test]
        public async Task PostWizdle_WithNonLetterInputs_ReturnsResponseWithWordsIgnoringNonLetterInputs()
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

            var request = new WizdleRequest
            {
                CorrectLetters = "1!",
                MisplacedLetters = "2£",
                ExcludeLetters = "3>",
            };

            // Act
            HttpResponseMessage response = httpClient.PostAsJsonAsync("/wizdle", request).Result;

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                WizdleResponse? result = response.Content.ReadFromJsonAsync<WizdleResponse>().Result;

                Assert.That(result?.Messages, Is.EqualTo(["Found 2334 Word(s) matching the criteria."]));
                Assert.That(result?.Words, Is.Not.Empty);
            }
        }
    }
}