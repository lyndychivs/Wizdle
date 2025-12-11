namespace Wizdle.Web.Functional.Tests.Hooks;

using System;
using System.Net.Http;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

using Projects;

using Reqnroll;

[Binding]
internal sealed class ContainerHook
{
    [BeforeTestRun]
    public static async Task Do()
    {
        IContainer apiContainer = new ContainerBuilder()
            .WithImage("wizdle-api:latest")
            .WithName($"{nameof(Wizdle_Api)}-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithPortBinding(9090, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            .Build();

        await apiContainer.StartAsync().ConfigureAwait(false);

        IContainer webContainer = new ContainerBuilder()
            .WithImage("wizdle-web:latest")
            .WithName($"{nameof(Wizdle_Web)}-{Guid.NewGuid()}")
            .DependsOn(apiContainer)
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            .Build();

        await webContainer.StartAsync().ConfigureAwait(false);

        using var httpClient = new HttpClient();

        var requestUri = new UriBuilder(Uri.UriSchemeHttp, apiContainer.Hostname, apiContainer.GetMappedPublicPort(9090), "health").Uri;

        var health = await httpClient.GetStringAsync(requestUri).ConfigureAwait(false);

        Console.WriteLine(health);
    }
}
