namespace Wizdle.Aspire.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using global::Aspire.Hosting.ApplicationModel;
    using global::Aspire.Hosting.Testing;

    using Microsoft.Extensions.DependencyInjection;

    using NUnit.Framework;

    [TestFixture]
    public class WebTests
    {
        [Test]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange
            IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Wizdle_Aspire_AppHost>();
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            await using global::Aspire.Hosting.DistributedApplication app = await appHost.BuildAsync();
            ResourceNotificationService resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
            await app.StartAsync();

            // Act
            HttpClient httpClient = app.CreateHttpClient("webfrontend");
            await resourceNotificationService.WaitForResourceAsync("webfrontend", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
            HttpResponseMessage response = await httpClient.GetAsync("/");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
