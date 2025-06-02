namespace Wizdle.ApiService.IntegrationTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Aspire.Hosting;
    using Aspire.Hosting.Testing;

    using NUnit.Framework;

    using Projects;

    [TestFixture]
    public class GetWizdleTests
    {
        [Test]
        public async Task GetWizdle_WhenCalled_ReturnsOkResult()
        {
            // Arrange
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            HttpClient httpClient = app.CreateHttpClient("api");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", cts.Token);

            // Act
            HttpResponseMessage response = await httpClient.GetAsync("/wizdle");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Is.EqualTo(string.Empty));
            }
        }
    }
}