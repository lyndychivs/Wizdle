namespace Wizdle.Web.Functional.Tests.Hooks;

using System;
using System.Threading;
using System.Threading.Tasks;

using Aspire.Hosting;
using Aspire.Hosting.Testing;

using Projects;

using Reqnroll;
using Reqnroll.BoDi;

[Binding]
internal static class EndpointSetupHook
{
    private const string ApiResourceName = "wizdle-api";

    private const string WebResourceName = "wizdle-web";

    [BeforeScenario(Order = int.MinValue)]
    public static async Task CreateEndpoint(ObjectContainer objectContainer)
    {
        IDistributedApplicationTestingBuilder builder = await DistributedApplicationTestingBuilder.CreateAsync<Wizdle_AppHost>();

        await using DistributedApplication app = await builder.BuildAsync();
        await app.StartAsync();

        using var ctsApi = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await app.ResourceNotifications.WaitForResourceHealthyAsync(ApiResourceName, ctsApi.Token);

        using var ctsWeb = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await app.ResourceNotifications.WaitForResourceHealthyAsync(WebResourceName, ctsWeb.Token);

        objectContainer.RegisterInstanceAs(app);
    }
}
