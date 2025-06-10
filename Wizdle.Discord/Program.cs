namespace Wizdle.Discord
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using NetCord.Hosting.Gateway;
    using NetCord.Hosting.Services;
    using NetCord.Hosting.Services.ApplicationCommands;

    using Wizdle.ServiceDefaults;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.AddServiceDefaults();

            builder.Services
                .AddDiscordGateway()
                .AddApplicationCommands()
                .AddHttpClient<WizdleApiClient>(client =>
                {
                    client.BaseAddress = new Uri("https+http://api");
                });

            IHost host = builder.Build();

            host.AddModules(typeof(Program).Assembly);

            host.UseGatewayEventHandlers();

            await host.RunAsync();
        }
    }
}