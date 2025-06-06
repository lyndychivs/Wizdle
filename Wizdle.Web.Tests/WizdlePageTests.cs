namespace Wizdle.Web.Tests
{
    using System;
    using System.IO;
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
        [SetUp]
        public async Task Setup()
        {
            await Context.Tracing.StartAsync(new ()
            {
                Title = TestContext.CurrentContext.Test.ClassName + "." + TestContext.CurrentContext.Test.Name,
                Screenshots = true,
                Snapshots = true,
                Sources = true,
                Name = TestContext.CurrentContext.Test.ClassName + "." + TestContext.CurrentContext.Test.Name,
            });
        }

        [TearDown]
        public async Task TearDown()
        {
            bool isFailed = TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Error
            || TestContext.CurrentContext.Result.Outcome == NUnit.Framework.Interfaces.ResultState.Failure;

            string tracingFilePath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "playwright-traces",
                $"{TestContext.CurrentContext.Test.ClassName}.{TestContext.CurrentContext.Test.Name}.zip");

            await Context.Tracing.StopAsync(new ()
            {
                Path = isFailed ? tracingFilePath : null,
            });

            TestContext.AddTestAttachment(tracingFilePath, "Playwright Trace");
        }

        [Test]
        public async Task WizdlePage_WhenNavigated_ExpectedElementsAreDisplayed()
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

            await Expect(Page.GetByRole(AriaRole.Img, new () { Name = "Wizdle" })).ToBeVisibleAsync();

            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Correct Letter 1" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Correct Letter 2" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Correct Letter 3" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Correct Letter 4" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Correct Letter 5" })).ToBeVisibleAsync();

            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Misplaced Letter 1" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Misplaced Letter 2" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Misplaced Letter 3" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Misplaced Letter 4" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Misplaced Letter 5" })).ToBeVisibleAsync();

            await Expect(Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = "Excluded Letters" })).ToBeVisibleAsync();

            await Expect(Page.GetByRole(AriaRole.Button, new () { Name = "Search" })).ToBeVisibleAsync();
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