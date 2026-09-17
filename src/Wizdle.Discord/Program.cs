namespace Wizdle.Discord;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;

using Wizdle.ServiceDefaults;

/// <summary>
/// The entry point for the Wizdle Discord bot host.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.AddServiceDefaults();

        builder.Services
            .AddDiscordGateway()
            .AddApplicationCommands()
            .AddHttpClient<WizdleApiClient>(client => client.BaseAddress = new Uri("https+http://wizdle-api"));

        IHost host = builder.Build();

        host.AddModules(typeof(Program).Assembly);

        await host.RunAsync();
    }
}
