namespace Wizdle.Web.Functional.Tests.Pages;

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Playwright;

internal abstract class BasePage
{
    protected const float DefaultTimeout = 30_000;

    protected const float VisibilityTimeout = 5_000;

    private const string ButtonDarkmode = "Darkmode";

    protected BasePage(IPage page, string expectedTitle)
    {
        Page = page ?? throw new ArgumentNullException(nameof(page));

        if (string.IsNullOrWhiteSpace(expectedTitle))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(expectedTitle));
        }

        string actualTitle = Page.TitleAsync().GetAwaiter().GetResult();
        if (!string.Equals(actualTitle, expectedTitle, StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"Expected Page Title to be \"{expectedTitle}\", but was \"{actualTitle}\".");
        }
    }

    protected IPage Page { get; private set; }

    public async Task<bool> IsDarkmodeButtonVisible()
    {
        return await IsButtonVisibile(ButtonDarkmode);
    }

    public async Task ClickDarkModeButton()
    {
        await ClickButton(ButtonDarkmode);
    }

    public async Task<bool> IsDarkmodeEnabled()
    {
        return await IsBackgroundColorMatching("rgb(50, 51, 61)");
    }

    public async Task<bool> IsDefaultBackgroundColor()
    {
        return await IsBackgroundColorMatching("rgb(255, 255, 255)");
    }

    public async Task<bool> DoesPageContainText(string text)
    {
        await WaitForNetworkIdle();

        try
        {
            await Page.GetByText(text).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = VisibilityTimeout,
            });

            return await Page.GetByText(text).IsVisibleAsync();
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    public async Task<string> GetScreenshot()
    {
        string filePath = Path.Combine("screenshots", $"screenshot-{Guid.NewGuid()}.png");
        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            FullPage = true,
            Path = filePath,
        });

        return filePath;
    }

    protected async Task WaitForNetworkIdle()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions()
        {
            Timeout = DefaultTimeout,
        });
    }

    protected async Task ClickButton(string buttonName)
    {
        if (string.IsNullOrWhiteSpace(buttonName))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(buttonName));
        }

        await WaitForNetworkIdle();

        ILocator button = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions()
        {
            Name = buttonName,
        });

        await button.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = VisibilityTimeout,
        });

        await button.ClickAsync();
    }

    protected async Task SetTextBox(string textBoxName, string text)
    {
        if (string.IsNullOrWhiteSpace(textBoxName))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(textBoxName));
        }

        ILocator textBox = Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions()
        {
            Name = textBoxName,
        });

        await textBox.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = VisibilityTimeout,
        });

        await textBox.FillAsync(text);
    }

    protected async Task<bool> IsButtonVisibile(string buttonName)
    {
        return await IsAriaRoleVisible(buttonName, AriaRole.Button);
    }

    protected async Task<bool> IsImageVisible(string imageName)
    {
        return await IsAriaRoleVisible(imageName, AriaRole.Img);
    }

    protected async Task<bool> IsTextBoxVisible(string textBoxName)
    {
        return await IsAriaRoleVisible(textBoxName, AriaRole.Textbox);
    }

    private async Task<bool> IsAriaRoleVisible(string textBoxName, AriaRole ariaRole)
    {
        if (string.IsNullOrWhiteSpace(textBoxName))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(textBoxName));
        }

        await WaitForNetworkIdle();

        try
        {
            ILocator locator = Page.GetByRole(ariaRole, new PageGetByRoleOptions()
            {
                Name = textBoxName,
            });

            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = VisibilityTimeout,
            });

            return await locator.IsVisibleAsync();
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    private async Task<bool> IsBackgroundColorMatching(string expectedColor)
    {
        await WaitForNetworkIdle();

        string backgroundColor = await Page.Locator("body")
            .EvaluateAsync<string>("element => window.getComputedStyle(element).backgroundColor");
        return string.Equals(backgroundColor, expectedColor, StringComparison.Ordinal);
    }
}
