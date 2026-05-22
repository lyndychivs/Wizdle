namespace Wizdle.Api.Functional.Tests.Setup;

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;

internal static class ContainerSetup
{
    public static async Task<string> GetWizdleApiUrl(int permitLimit = 60, int windowSeconds = 60)
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
            .WithEnvironment("RateLimiting__PermitLimit", permitLimit.ToString(CultureInfo.InvariantCulture))
            .WithEnvironment("RateLimiting__WindowSeconds", windowSeconds.ToString(CultureInfo.InvariantCulture))
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080).ForPath("/health")))
            .Build();

        await container.StartAsync();

        return $"http://localhost:{container.GetMappedPublicPort(8080)}";
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
