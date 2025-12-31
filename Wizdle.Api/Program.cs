namespace Wizdle.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Scalar.AspNetCore;

using Wizdle;
using Wizdle.Models;
using Wizdle.ServiceDefaults;

internal sealed class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddProblemDetails();
        builder.Services.AddSingleton<WizdleEngine>();
        builder.Services.AddOpenApi();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        WebApplication app = builder.Build();

        app.UseExceptionHandler();

        app.MapPost("/", ([FromBody] WizdleRequest request, WizdleEngine engine) =>
        {
            if (request is null)
            {
                return Results.BadRequest($"{nameof(WizdleRequest)} cannot be null.");
            }

            return Results.Ok(engine.ProcessWizdleRequest(request));
        }).WithName("PostWizdle")
        .Produces<WizdleResponse>()
        .WithSummary("Processes a Wizdle request in an attempt to solve the possible words.");

        app.MapDefaultEndpoints();

        app.MapOpenApi();
        app.MapScalarApiReference();

        app.Run();
    }
}
