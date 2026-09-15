namespace Wizdle.Discord;

using System;

using Microsoft.Extensions.Logging;

using NetCord.Services.ApplicationCommands;

public sealed partial class HelpSlashCommand(ILogger<HelpSlashCommand> logger)
    : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("help", "Help with Wizdle")]
    public string GetHelp()
    {
        LogReceivedInteraction(
            logger,
            Context.Interaction.Data.Name,
            Context.User.Username,
            Context.User.Id);

        return $"# 🟩 Wizdle — `/word` parameters{Environment.NewLine}" +
            $"🟩 **`correct`** — right position (`?r???` = R is 2nd){Environment.NewLine}" +
            $"🟨 **`misplaced`** — wrong position (`??t??` = T isn't 3rd){Environment.NewLine}" +
            $"⬛ **`exclude`** — not in word (`abc` = no A, B, C){Environment.NewLine}" +
            $"> Use `?` for unknown letters";
    }

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Received {Interaction} request from {Username} {UserId}")]
    static partial void LogReceivedInteraction(
        ILogger logger,
        string interaction,
        string username,
        ulong userId);
}
