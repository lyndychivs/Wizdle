namespace Wizdle.Web.Functional.Tests.Pages;

using System;
using System.Threading.Tasks;

using Microsoft.Playwright;

internal abstract class BasePage
{
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
        return await Page.GetByText(text).IsVisibleAsync();
    }

    protected async Task ClickButton(string buttonName)
    {
        if (string.IsNullOrWhiteSpace(buttonName))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(buttonName));
        }

        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions() { Name = buttonName }).ClickAsync();
    }

    protected async Task SetTextBox(string textBoxName, string text)
    {
        if (string.IsNullOrWhiteSpace(textBoxName))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(textBoxName));
        }

        await Page.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions() { Name = textBoxName }).FillAsync(text);
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
        return string.IsNullOrWhiteSpace(textBoxName)
            ? throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(textBoxName))
            : await Page.GetByRole(ariaRole, new PageGetByRoleOptions() { Name = textBoxName }).IsVisibleAsync();
    }

    private async Task<bool> IsBackgroundColorMatching(string expectedColor)
    {
        string backgroundColor = await Page.Locator("body").EvaluateAsync<string>("element => window.getComputedStyle(element).backgroundColor");
        return string.Equals(backgroundColor, expectedColor, StringComparison.Ordinal);
    }
}
