namespace Wizdle.Api;

using System;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Scalar.AspNetCore;

using Wizdle;
using Wizdle.Models;
using Wizdle.ServiceDefaults;

internal sealed class Program
{
    private const string RateLimitingPolicy = "fixed-window";

    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        builder.Services.AddProblemDetails();
        builder.Services.AddSingleton<WizdleEngine>();
        builder.Services.AddOpenApi();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddRateLimiter(rateLimiterOptions =>
        {
            int permitLimit = builder.Configuration.GetValue("RateLimiting:PermitLimit", 60);
            int windowSeconds = builder.Configuration.GetValue("RateLimiting:WindowSeconds", 60);

            /*
             * Partitioning by httpContext.Connection.RemoteIpAddress will rate-limit the reverse proxy/load balancer IP when the API is deployed behind a proxy, effectively applying a shared limit to all clients.
             * If this service is expected to run behind a proxy, configure forwarded headers (and trust configuration) so the effective client IP is used, or partition on a different client identifier.
             */

            rateLimiterOptions.AddPolicy(RateLimitingPolicy, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = TimeSpan.FromSeconds(windowSeconds),
                        QueueLimit = 0,
                    }));

            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });

        WebApplication app = builder.Build();

        app.UseExceptionHandler();
        app.UseRateLimiter();

        app.MapPost("/", ([FromBody] WizdleRequest request, WizdleEngine engine) =>
        {
            if (request is null)
            {
                return Results.BadRequest($"{nameof(WizdleRequest)} cannot be null.");
            }

            return Results.Ok(engine.ProcessWizdleRequest(request));
        }).WithName("PostWizdle")
        .Produces<WizdleResponse>()
        .WithSummary("Processes a Wizdle request in an attempt to solve the possible words.")
        .RequireRateLimiting(RateLimitingPolicy);

        app.MapDefaultEndpoints();

        app.MapOpenApi();
        app.MapScalarApiReference();

        app.Run();
    }
}
