namespace Wizdle.Web.Functional.Tests.Pages;

using System;
using System.Threading.Tasks;

using Microsoft.Playwright;

internal abstract class BasePage
{
    protected BasePage(IPage page)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));
    }

    protected IPage Page { get; private set; }

    public async Task NavigateTo(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(url));
        }

        await Page.GotoAsync(url, new PageGotoOptions() { WaitUntil = WaitUntilState.NetworkIdle });
    }
}
