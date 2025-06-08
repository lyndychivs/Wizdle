namespace Wizdle.Discord
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
            });

            WebApplication app = builder.Build();

            app.Run();
        }
    }
}