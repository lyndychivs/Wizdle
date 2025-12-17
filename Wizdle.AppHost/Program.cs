namespace Wizdle.AppHost;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.Configuration;

using Projects;

internal sealed class Program
{
    private const string WebServiceName = "wizdle-web";

    private const string ApiServiceName = "wizdle-api";

    private const string DiscordServiceName = "wizdle-discord";

    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        builder.AddDockerComposeEnvironment("wizdle")
            .WithDashboard(dashboard => dashboard.WithHostPort(8080)
                .WithForwardedHeaders(true));

        IResourceBuilder<ProjectResource> apiService = builder.AddProject<Wizdle_Api>(ApiServiceName)
            .WithExternalHttpEndpoints()
            .WithScalarDocs()
            .PublishAsDockerComposeService((resource, service) => service.Name = ApiServiceName);

        builder.AddProject<Wizdle_Web>(WebServiceName)
            .WithExternalHttpEndpoints()
            .WithReference(apiService)
            .WaitFor(apiService)
            .PublishAsDockerComposeService((resource, service) => service.Name = WebServiceName);

        if (builder.Configuration.GetValue<bool>("EnableDiscord"))
        {
            builder.AddProject<Wizdle_Discord>(DiscordServiceName)
                .WithExternalHttpEndpoints()
                .WithReference(apiService)
                .WaitFor(apiService).PublishAsDockerComposeService((resource, service) => service.Name = DiscordServiceName);
        }

        builder.Build().Run();
    }
}
