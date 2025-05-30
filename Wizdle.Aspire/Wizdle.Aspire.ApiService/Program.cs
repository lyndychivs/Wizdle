namespace Wizdle.Aspire.ApiService
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using Scalar.AspNetCore;

    using Wizdle;
    using Wizdle.Aspire.ServiceDefaults;
    using Wizdle.Models;

    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            builder.Services.AddProblemDetails();
            builder.Services.AddSingleton<WizdleEngine>();
            builder.Services.AddOpenApi();

            WebApplication app = builder.Build();

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

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.Run();
        }
    }
}