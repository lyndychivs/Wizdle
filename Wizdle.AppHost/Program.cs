namespace Wizdle.AppHost;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.Configuration;

using Projects;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        IResourceBuilder<ProjectResource> apiService = builder.AddProject<Wizdle_Api>("api")
            .WithScalarDocs();

        builder.AddProject<Wizdle_Web>("web")
            .WithExternalHttpEndpoints()
            .WithReference(apiService)
            .WaitFor(apiService);

        if (builder.Configuration.GetValue<bool>("EnableDiscord"))
        {
            builder.AddProject<Wizdle_Discord>("discord")
                .WithExternalHttpEndpoints()
                .WithReference(apiService)
                .WaitFor(apiService);
        }

        builder.Build().Run();
    }
}
