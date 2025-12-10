namespace Wizdle.Web.Functional.Tests.Hooks;

using System.Threading.Tasks;

using Microsoft.Playwright;

using Reqnroll;
using Reqnroll.BoDi;

[Binding]
internal static class BrowserHook
{
    [BeforeScenario]
    public static async Task CreateBrowserInstance(ObjectContainer objectContainer)
    {
        IPlaywright playwright = await Playwright.CreateAsync();
        IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });

        IPage page = browser.NewContextAsync().GetAwaiter().GetResult()
            .NewPageAsync().GetAwaiter().GetResult();

        objectContainer.RegisterInstanceAs(page);
    }
}
