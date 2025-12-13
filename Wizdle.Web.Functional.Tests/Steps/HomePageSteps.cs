namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Threading.Tasks;

using Reqnroll;

using Wizdle.Web.Functional.Tests.Data;
using Wizdle.Web.Functional.Tests.Pages;

[Binding]
internal sealed class HomePageSteps
{
    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    private readonly WizdleTestData _wizdleTestData;

    private readonly HomePage _homePage;

    public HomePageSteps(IReqnrollOutputHelper reqnrollOutputHelper, WizdleTestData wizdleTestData, HomePage homePage)
    {
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
        _wizdleTestData = wizdleTestData ?? throw new ArgumentNullException(nameof(wizdleTestData));
        _homePage = homePage ?? throw new ArgumentNullException(nameof(homePage));
    }

    [StepDefinition("I navigate to the Wizdle Home page")]
    public async Task NavigateToHomePage()
    {
        _reqnrollOutputHelper.WriteLine($"Navigating to {_wizdleTestData.Url}");
        await _homePage.NavigateTo(_wizdleTestData.Url);
    }
}
