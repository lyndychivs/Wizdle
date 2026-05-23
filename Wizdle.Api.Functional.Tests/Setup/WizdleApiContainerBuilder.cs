namespace Wizdle.Api.Functional.Tests.Setup;

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

internal sealed class WizdleApiContainerBuilder
{
    private int _permitLimit = 60;
    private int _windowSeconds = 60;

    public WizdleApiContainerBuilder WithPermitLimit(int permitLimit)
    {
        _permitLimit = permitLimit;
        return this;
    }

    public WizdleApiContainerBuilder WithWindowSeconds(int windowSeconds)
    {
        _windowSeconds = windowSeconds;
        return this;
    }

    public async Task<ContainerHandle> BuildAsync()
    {
        string repositoryRoot = GetRepositoryRoot();

        IFutureDockerImage futureDockerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(repositoryRoot)
            .WithDockerfile("Wizdle.Api/Dockerfile")
            .WithCleanUp(true)
            .Build();

        await futureDockerImage.CreateAsync();

        IContainer container = new ContainerBuilder(futureDockerImage)
            .WithName($"wizdle-api-{Guid.NewGuid()}")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithPortBinding(8080, assignRandomHostPort: true)
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Production")
            .WithEnvironment("RateLimiting__PermitLimit", _permitLimit.ToString(CultureInfo.InvariantCulture))
            .WithEnvironment("RateLimiting__WindowSeconds", _windowSeconds.ToString(CultureInfo.InvariantCulture))
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080).ForPath("/health")))
            .Build();

        await container.StartAsync();

        return new ContainerHandle(
            new Uri($"http://localhost:{container.GetMappedPublicPort(8080)}"),
            container);
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
