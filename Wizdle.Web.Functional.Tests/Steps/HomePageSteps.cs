namespace Wizdle.Web.Functional.Tests.Steps;

using System;

using Microsoft.Playwright;

using NUnit.Framework;

using Reqnroll;

[Binding]
internal sealed class HomePageSteps
{
    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    private readonly IPage _page;

    public HomePageSteps(IReqnrollOutputHelper reqnrollOutputHelper, IPage page)
    {
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
        _page = page ?? throw new ArgumentNullException(nameof(page));
    }

    [StepDefinition("the Page title should be {string}")]
    public void AssertPageTitleShouldBe(string expectedTitle)
    {
        _reqnrollOutputHelper.WriteLine($"Checking Page Title for \"{expectedTitle}\"");

        Assert.That(_page.TitleAsync().Result, Is.EqualTo(expectedTitle));
    }
}
