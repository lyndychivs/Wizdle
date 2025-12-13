namespace Wizdle.Web.Functional.Tests.Extensions;

using System;
using System.Threading.Tasks;

using Microsoft.Playwright;

using NUnit.Framework;

internal static class PageExtensions
{
    public static async Task AssertTextBoxIsVisible(this IPage page, string textBoxName)
    {
        await AssertAriaRoleIsVisible(page, textBoxName, AriaRole.Textbox);
    }

    private static async Task AssertAriaRoleIsVisible(IPage page, string textBoxName, AriaRole ariaRole)
    {
        if (string.IsNullOrWhiteSpace(textBoxName))
        {
            throw new ArgumentException("Value cannot be null, empty or whitespace", nameof(textBoxName));
        }

        bool isVisible = await page.GetByRole(ariaRole, new PageGetByRoleOptions() { Name = textBoxName }).IsVisibleAsync();

        Assert.That(isVisible, Is.True, $"Expected {ariaRole} (\"{textBoxName}\") to be visible, but it was not.");
    }
}
