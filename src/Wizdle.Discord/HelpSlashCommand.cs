namespace Wizdle.Discord;

using System;

using Microsoft.Extensions.Logging;

using NetCord.Services.ApplicationCommands;

/// <summary>
/// The Discord slash command for displaying help with Wizdle.
/// </summary>
/// <param name="logger">The <see cref="ILogger{HelpSlashCommand}"/> interface to use.</param>
public sealed partial class HelpSlashCommand(ILogger<HelpSlashCommand> logger)
    : ApplicationCommandModule<ApplicationCommandContext>
{
    /// <summary>
    /// Gets the help text describing the <c>/word</c> command parameters.
    /// </summary>
    /// <returns>A formatted message explaining how to use the <c>/word</c> command.</returns>
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
