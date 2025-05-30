namespace Wizdle.Aspire.ApiService
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using Wizdle;
    using Wizdle.Aspire.ServiceDefaults;
    using Wizdle.Models;

    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add service defaults & Aspire client integrations.
            builder.AddServiceDefaults();

            // Add services to the container.
            builder.Services.AddProblemDetails();
            builder.Services.AddSingleton<WizdleEngine>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseExceptionHandler();

            app.MapGet("/wizdle", () => Results.Ok())
                .WithName("GetWizdle");

            app.MapPost("/wizdle", ([FromBody] WizdleRequest request, WizdleEngine engine) =>
            {
                if (request is null)
                {
                    return Results.BadRequest($"{nameof(WizdleRequest)} cannot be null.");
                }

                return Results.Ok(engine.ProcessWizdleRequest(request));
            }).WithName("PostWizdle");

            app.MapDefaultEndpoints();

            app.Run();
        }
    }
}