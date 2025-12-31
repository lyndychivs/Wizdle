namespace Wizdle.AppHost;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using Microsoft.Extensions.Diagnostics.HealthChecks;

internal static class ResourceBuilderExtensions
{
    internal static IResourceBuilder<T> WithScalarDocs<T>(this IResourceBuilder<T> resourceBuilder)
        where T : IResourceWithEndpoints
    {
        return resourceBuilder.WithCommand(
            "scalar-docs",
            "Scalar API Documentation",
            _ => OpenScalarApiDocumentation(resourceBuilder),
            new CommandOptions
            {
                Description = "Opens the Scalar API Documentation.",
                IconName = "Document",
                IconVariant = IconVariant.Filled,
                IsHighlighted = true,
                UpdateState = context => context.ResourceSnapshot.HealthStatus == HealthStatus.Healthy ? ResourceCommandState.Enabled : ResourceCommandState.Disabled,
            });
    }

    private static async Task<ExecuteCommandResult> OpenScalarApiDocumentation<T>(IResourceBuilder<T> resourceBuilder)
        where T : IResourceWithEndpoints
    {
        return await Task.Run(() =>
        {
            try
            {
                EndpointReference endpoint = resourceBuilder.GetEndpoint("https");
                string url = $"{endpoint.Url}/scalar/v1";

                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true,
                });

                return new ExecuteCommandResult
                {
                    Success = true,
                };
            }
            catch (Exception ex)
            {
                return new ExecuteCommandResult
                {
                    Success = false,
                    ErrorMessage = ex.ToString(),
                };
            }
        });
    }
}
