namespace Wizdle.Discord;

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;

using Wizdle.ServiceDefaults;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.AddServiceDefaults();

        builder.Services
            .AddDiscordGateway(op => op.Intents = GatewayIntents.Guilds)
            .AddGatewayHandlers(typeof(Program).Assembly)
            .AddApplicationCommands()
            .AddHttpClient<WizdleApiClient>(client => client.BaseAddress = new Uri("https+http://wizdle-api"));

        IHost host = builder.Build();

        await host.RunAsync();
    }
}
