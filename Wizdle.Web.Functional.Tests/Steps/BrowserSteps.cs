namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Threading.Tasks;

using Microsoft.Playwright;

using NUnit.Framework;

using Reqnroll;

using Wizdle.Web.Functional.Tests.Hooks;

[Binding]
internal sealed class BrowserSteps
{
    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    private readonly IPage _page;

    private readonly IContainerHandle _containerHandle;

    public BrowserSteps(
        IReqnrollOutputHelper reqnrollOutputHelper,
        IPage page,
        IContainerHandle containerHandle)
    {
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
        _page = page ?? throw new ArgumentNullException(nameof(page));
        _containerHandle = containerHandle ?? throw new ArgumentNullException(nameof(containerHandle));
    }

    [StepDefinition("I navigate to the Wizdle Home page")]
    public async Task NavigateToHomePage()
    {
        _reqnrollOutputHelper.WriteLine($"Navigating to {_containerHandle.Url}");

        await _page.GotoAsync(_containerHandle.Url.AbsoluteUri, new PageGotoOptions()
        {
            WaitUntil = WaitUntilState.NetworkIdle,
            Timeout = 30_000,
        });
    }

    [StepDefinition("the Page title should be {string}")]
    public async Task AssertPageTitleShouldBe(string expectedTitle)
    {
        string actualTitle = await _page.TitleAsync();
        Assert.That(
            actualTitle,
            Is.EqualTo(expectedTitle),
            $"Expected Page Title \"{expectedTitle}\", but was \"{actualTitle}\".");
    }
}
