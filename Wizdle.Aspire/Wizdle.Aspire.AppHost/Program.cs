using Projects;

internal class Program
{
    private static void Main(string[] args)
    {
        IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

        IResourceBuilder<ProjectResource> apiService = builder.AddProject<Wizdle_Aspire_ApiService>("apiservice");

        builder.AddProject<Wizdle_Aspire_Web>("webfrontend")
            .WithExternalHttpEndpoints()
            .WithReference(apiService)
            .WaitFor(apiService);

        builder.Build().Run();
    }
}