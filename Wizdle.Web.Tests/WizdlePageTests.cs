namespace Wizdle.Web.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Aspire.Hosting;
    using Aspire.Hosting.Testing;

    using Microsoft.Playwright;
    using Microsoft.Playwright.NUnit;

    using NUnit.Framework;

    using Projects;

    [TestFixture]
    public class WizdlePageTests : PageTest
    {
        [Test]
        public async Task WizdlePage_WhenNavigated_ReturnsPageTitle()
        {
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            using var ctsApi = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", ctsApi.Token);

            using var ctsWeb = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("web", ctsWeb.Token);

            await Page.GotoAsync(app.GetEndpoint("web", "https").ToString(), new () { WaitUntil = WaitUntilState.NetworkIdle });

            await Expect(Page).ToHaveTitleAsync("Wizdle | Solve Wordle...");
        }

        [Test]
        public async Task WizdlePage_WhenDarkmodeIsClicked_ReturnsPageInDarkmode()
        {
            IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Wizdle_AppHost>();

            await using DistributedApplication app = await builder.BuildAsync();
            await app.StartAsync();

            using var ctsApi = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("api", ctsApi.Token);

            using var ctsWeb = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await app.ResourceNotifications.WaitForResourceHealthyAsync("web", ctsWeb.Token);

            await Page.GotoAsync(app.GetEndpoint("web", "https").ToString(), new () { WaitUntil = WaitUntilState.NetworkIdle });

            await Expect(Page.Locator("css=body")).ToHaveCSSAsync("background-color", "rgb(255, 255, 255)");

            await Page.GetByRole(AriaRole.Button, new () { Name = "Darkmode" }).ClickAsync();

            await Expect(Page.Locator("css=body")).ToHaveCSSAsync("background-color", "rgb(50, 51, 61)");
        }
    }
}