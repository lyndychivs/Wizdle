namespace Wizdle.Web.Functional.Tests.Hooks;

using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

using Projects;

using Reqnroll;

using Wizdle.Web.Functional.Tests.Models;

[Binding]
internal sealed class ContainerHook
{
    [BeforeTestRun]
    public static async Task CreateWizdleContainers(Endpoint endpoint)
    {
        INetwork network = new NetworkBuilder()
            .WithName($"wizdle-network-{Guid.NewGuid()}")
            .WithCleanUp(true)
            .Build();

        await network.CreateAsync().ConfigureAwait(false);

        IContainer apiContainer = new ContainerBuilder()
            .WithImage("wizdle-api:latest")
            .WithName($"{nameof(Wizdle_Api)}-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithNetwork(network)
            .WithNetworkAliases("wizdle-api")
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080).ForPath("/health")))
            .Build();

        await apiContainer.StartAsync().ConfigureAwait(false);

        IContainer webContainer = new ContainerBuilder()
            .WithImage("wizdle-web:latest")
            .WithName($"{nameof(Wizdle_Web)}-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithNetwork(network)
            .WithEnvironment("ASPNETCORE_URLS", "http://+:8080")
            .WithEnvironment("services__wizdle-api__http__0", "http://wizdle-api:8080")
            .WithEnvironment("services__wizdle-api__https__0", "http://wizdle-api:8080")
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
            .DependsOn(apiContainer)
            .Build();

        await webContainer.StartAsync().ConfigureAwait(false);

        endpoint.Url = $"http://localhost:{webContainer.GetMappedPublicPort(8080)}";
    }
}
