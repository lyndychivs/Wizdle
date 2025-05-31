namespace Wizdle.Aspire.AppHost
{
    using global::Aspire.Hosting;
    using global::Aspire.Hosting.ApplicationModel;

    using Projects;

    internal class Program
    {
        private static void Main(string[] args)
        {
            IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

            IResourceBuilder<ProjectResource> apiService = builder.AddProject<Wizdle_Aspire_ApiService>("api")
                .WithScalarDocs();

            builder.AddProject<Wizdle_Aspire_Web>("web")
                .WithExternalHttpEndpoints()
                .WithReference(apiService)
                .WaitFor(apiService);

            builder.Build().Run();
        }
    }
}