namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Threading.Tasks;

using Microsoft.Playwright;

using Reqnroll;

using Wizdle.Web.Functional.Tests.Data;

[Binding]
internal sealed class NavigationSteps
{
    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    private readonly WizdleTestData _wizdleTestData;

    private readonly IPage _page;

    public NavigationSteps(IReqnrollOutputHelper reqnrollOutputHelper, WizdleTestData testData, IPage page)
    {
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
        _wizdleTestData = testData ?? throw new ArgumentNullException(nameof(testData));
        _page = page ?? throw new ArgumentNullException(nameof(page));
    }

    [StepDefinition("I navigate to the Wizdle Url")]
    public async Task GoToWizdleUrl()
    {
        _reqnrollOutputHelper.WriteLine($"Navigating to {_wizdleTestData.Url}");
        await _page.GotoAsync(_wizdleTestData.Url);
    }
}
