namespace Wizdle.AppHost;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.Configuration;

using Projects;

internal static class Program
{
    private const string WebServiceName = "wizdle-web";

    private const string ApiServiceName = "wizdle-api";

    private const string DiscordServiceName = "wizdle-discord";

    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        if (!builder.ExecutionContext.IsPublishMode)
        {
            builder.AddDockerComposeEnvironment("wizdle")
                .WithDashboard(dashboard => dashboard.WithHostPort(8080)
                    .WithForwardedHeaders(true));
        }

        IResourceBuilder<ProjectResource> apiService = builder.AddProject<Wizdle_Api>(ApiServiceName)
            .WithExternalHttpEndpoints()
            .WithScalarDocs();

        if (!builder.ExecutionContext.IsPublishMode)
        {
            apiService.PublishAsDockerComposeService((resource, service) => service.Name = ApiServiceName);
        }

        IResourceBuilder<ProjectResource> webService = builder.AddProject<Wizdle_Web>(WebServiceName)
            .WithExternalHttpEndpoints()
            .WithReference(apiService)
            .WaitFor(apiService);

        if (!builder.ExecutionContext.IsPublishMode)
        {
            webService.PublishAsDockerComposeService((resource, service) => service.Name = WebServiceName);
        }

        if (builder.Configuration.GetValue<bool>("EnableDiscord"))
        {
            IResourceBuilder<ProjectResource> discordService = builder.AddProject<Wizdle_Discord>(DiscordServiceName)
                .WithExternalHttpEndpoints()
                .WithReference(apiService)
                .WaitFor(apiService)
                .WithEnvironment("Discord__Token", builder.Configuration["Discord:Token"] ?? string.Empty)
                .WithEnvironment("Discord__PublicKey", builder.Configuration["Discord:PublicKey"] ?? string.Empty);

            if (!builder.ExecutionContext.IsPublishMode)
            {
                discordService.PublishAsDockerComposeService((resource, service) => service.Name = DiscordServiceName);
            }
        }

        builder.Build().Run();
    }
}
