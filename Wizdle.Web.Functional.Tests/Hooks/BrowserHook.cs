namespace Wizdle.Web.Functional.Tests.Hooks;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Playwright;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using Reqnroll;
using Reqnroll.BoDi;

using Retry;

using Wizdle.Web.Functional.Tests.Configuration;

[Binding]
internal static class BrowserHook
{
    [BeforeScenario]
    public static async Task CreateBrowserInstance(
        IObjectContainer objectContainer,
        IScenarioContext scenarioContext,
        IReqnrollOutputHelper reqnrollOutputHelper)
    {
        var retry = new Retry(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(3));

        IPlaywrightConfiguration configuration = new RunSettingsPlaywrightConfiguration();

        reqnrollOutputHelper.WriteLine(configuration.ToString());

        IPlaywright playwright = null!;
        await retry.UntilAsync(async () =>
        {
            playwright = await Playwright.CreateAsync();
            return await Task.FromResult(playwright is not null);
        });

        if (playwright is null)
        {
            throw new InvalidOperationException("Failed to initialize Playwright.");
        }

        IBrowser browser = await playwright[configuration.BrowserName].LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = configuration.Headless,
            Channel = configuration.Channel,
        });

        IBrowserContext browserContext = await browser.NewContextAsync();
        IPage page = await browserContext.NewPageAsync();

        await page.Context.Tracing.StartAsync(new TracingStartOptions()
        {
            Title = scenarioContext.ScenarioInfo.Title,
            Screenshots = true,
            Snapshots = true,
            Sources = true,
            Name = scenarioContext.ScenarioInfo.Title,
        });

        objectContainer.RegisterInstanceAs(playwright);
        objectContainer.RegisterInstanceAs(browser);
        objectContainer.RegisterInstanceAs(browserContext);
        objectContainer.RegisterInstanceAs(page);
    }

    [AfterScenario]
    public static async Task DisposeBrowserInstance(
        IPlaywright playwright,
        IBrowser browser,
        IBrowserContext browserContext,
        IPage page,
        ScenarioContext scenarioContext,
        IReqnrollOutputHelper reqnrollOutputHelper)
    {
        bool isFailed = TestContext.CurrentContext.Result.Outcome == ResultState.Error
        || TestContext.CurrentContext.Result.Outcome == ResultState.Failure;

        string tracingFilePath = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "playwright-traces",
            $"{scenarioContext.ScenarioInfo.Title.GetOnlyValidLetters()}-{Guid.NewGuid()}.zip");

        await page.Context.Tracing.StopAsync(new TracingStopOptions()
        {
            Path = isFailed ? tracingFilePath : null,
        });

        if (isFailed)
        {
            TestContext.AddTestAttachment(tracingFilePath, "Playwright Trace");
            reqnrollOutputHelper.AddAttachment(tracingFilePath);
        }

        await page.CloseAsync();
        await browser.CloseAsync();
        await browserContext.CloseAsync();

        playwright.Dispose();
    }

    private static string GetOnlyValidLetters(this string text)
    {
        return string.Concat(text.Where(char.IsLetter).Take(25));
    }
}
