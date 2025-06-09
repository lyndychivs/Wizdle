namespace Wizdle.Discord
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Hosting;

    using NetCord.Hosting.Gateway;
    using NetCord.Hosting.Services.ApplicationCommands;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

            builder.Services
                .AddDiscordGateway()
                .AddApplicationCommands();

            IHost host = builder.Build();

            // Add commands using minimal APIs
            host.AddSlashCommand("word", "Search for possible words", () => "Wizdle!");

            host.UseGatewayEventHandlers();

            await host.RunAsync();
        }
    }
}