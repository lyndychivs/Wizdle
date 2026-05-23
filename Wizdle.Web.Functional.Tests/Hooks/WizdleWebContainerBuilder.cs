namespace Wizdle.Web.Functional.Tests.Hooks;

using System;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Networks;

using Wizdle.Web.Functional.Tests.Models;

internal sealed class WizdleWebContainerBuilder
{
    private readonly string _repositoryRoot;

    public WizdleWebContainerBuilder()
    {
        _repositoryRoot = GetRepositoryRoot();
    }

    public async Task<ContainerHandle> BuildAsync()
    {
        INetwork network = new NetworkBuilder()
            .WithName($"wizdle-network-{Guid.NewGuid()}")
            .WithCleanUp(true)
            .Build();

        await network.CreateAsync();

        IFutureDockerImage apiFutureDockerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(_repositoryRoot)
            .WithDockerfile("Wizdle.Api/Dockerfile")
            .WithCleanUp(true)
            .Build();

        await apiFutureDockerImage.CreateAsync();

        IContainer apiContainer = new ContainerBuilder(apiFutureDockerImage)
            .WithName($"wizdle-api-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithNetwork(network)
            .WithNetworkAliases("wizdle-api")
            .WithPortBinding(8080, assignRandomHostPort: true)
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080).ForPath("/health")))
            .Build();

        await apiContainer.StartAsync();

        IFutureDockerImage webFutureDockerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(_repositoryRoot)
            .WithDockerfile("Wizdle.Web/Dockerfile")
            .WithCleanUp(true)
            .Build();

        await webFutureDockerImage.CreateAsync();

        IContainer webContainer = new ContainerBuilder(webFutureDockerImage)
            .WithName($"wizdle-web-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithNetwork(network)
            .WithEnvironment("ASPNETCORE_URLS", "http://+:8080")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
            .WithEnvironment("services__wizdle-api__http__0", "http://wizdle-api:8080")
            .WithEnvironment("services__wizdle-api__https__0", "http://wizdle-api:8080")
            .WithPortBinding(8080, assignRandomHostPort: true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080).ForPath("/health")))
            .DependsOn(apiContainer)
            .Build();

        await webContainer.StartAsync();

        return new ContainerHandle(
            new Uri($"http://localhost:{webContainer.GetMappedPublicPort(8080)}"),
            apiContainer,
            webContainer,
            network);
    }

    private static string GetRepositoryRoot()
    {
        string? directory = Directory.GetCurrentDirectory();

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory, "Wizdle.slnx")))
            {
                return directory;
            }

            directory = Directory.GetParent(directory)?.FullName;
        }

        throw new InvalidOperationException("Could not find repository root (Wizdle.slnx file)");
    }
}
