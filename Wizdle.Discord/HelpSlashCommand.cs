namespace Wizdle.Discord;

using System;

using Microsoft.Extensions.Logging;

using NetCord.Services.ApplicationCommands;

public class HelpSlashCommand(ILogger<HelpSlashCommand> logger)
    : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("help", "Help with Wizdle")]
    public string GetHelp()
    {
        logger.LogInformation(
            "Received {Interaction} request from {Username} {Id}",
            Context.Interaction.Data.Name,
            Context.User.Username,
            Context.User.Id);

        return $"# Wizdle Help{Environment.NewLine}" +
            $"🔧 Commands:{Environment.NewLine}" +
            $"- `/words` supports 3 parameters:{Environment.NewLine}" +
            $"  - `correct`: The correct letters known to exist in the Word, follow the format of `a?b?c` where unknown letters are represented by a question mark (`?`){Environment.NewLine}" +
            $"  - `misplaced`: The misplaced letters known to exist in the Word, follow the format of `a?b?c` where unknown letters are represented by a question mark (`?`){Environment.NewLine}" +
            $"  - `exclude`: The letters known to not exist in the Word, follow the format of `abc`";
    }
}
