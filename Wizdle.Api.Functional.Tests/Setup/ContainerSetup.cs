namespace Wizdle.Api.Functional.Tests.Hooks;

using System;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

internal static class ContainerSetup
{
    public static async Task<string> GetWizdleApiUrl()
    {
        IContainer apiContainer = new ContainerBuilder()
            .WithImage("wizdle-api:latest")
            .WithName($"wizdle-api-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithPortBinding(8080, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080).ForPath("/health")))
            .Build();

        await apiContainer.StartAsync().ConfigureAwait(false);

        return $"http://localhost:{apiContainer.GetMappedPublicPort(8080)}";
    }
}
