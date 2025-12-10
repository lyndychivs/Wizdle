namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Threading.Tasks;

using Aspire.Hosting;
using Aspire.Hosting.Testing;

using Microsoft.Playwright;

using Reqnroll;

[Binding]
internal sealed class NavigationSteps
{
    private const string WebResourceName = "web";

    private const string EndpointName = "https";

    private readonly IPage _page;

    private readonly DistributedApplication _distributedApplication;

    public NavigationSteps(IPage page, DistributedApplication distributedApplication)
    {
        _page = page ?? throw new ArgumentNullException(nameof(page));
        _distributedApplication = distributedApplication ?? throw new ArgumentNullException(nameof(distributedApplication));
    }

    [StepDefinition("I navigate to the Wizdle Url")]
    public async Task GoToWizdleUrl()
    {
        string url = _distributedApplication.GetEndpoint(WebResourceName, EndpointName).ToString();
        await _page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });
    }
}
