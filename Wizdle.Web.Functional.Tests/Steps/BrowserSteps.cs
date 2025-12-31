namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Threading.Tasks;

using Microsoft.Playwright;

using NUnit.Framework;

using Reqnroll;

using Wizdle.Web.Functional.Tests.Models;

[Binding]
internal sealed class BrowserSteps
{
    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    private readonly IPage _page;

    private readonly Endpoint _endpoint;

    public BrowserSteps(IReqnrollOutputHelper reqnrollOutputHelper, IPage page, Endpoint endpoint)
    {
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
        _page = page ?? throw new ArgumentNullException(nameof(page));
        _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
    }

    [StepDefinition("I navigate to the Wizdle Home page")]
    public async Task NavigateToHomePage()
    {
        _reqnrollOutputHelper.WriteLine($"Navigating to {_endpoint.Url}");

        await _page.GotoAsync(_endpoint.Url, new PageGotoOptions()
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
