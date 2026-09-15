namespace Wizdle.Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NetCord.Services.ApplicationCommands;

using Wizdle.Models;

public sealed partial class WordSlashCommand(ILogger<WordSlashCommand> logger, WizdleApiClient wizdleApiClient)
    : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("word", "Find possible Wordle solution words")]
    public async Task<string> GetWordsAsync(
    [SlashCommandParameter(
        Name = "correct",
        Description = "🟩 Correct position, ? = unknown (e.g. ?r??? = R is 2nd letter)",
        MaxLength = 5,
        MinLength = 0)]
    string correctLetters = "",
    [SlashCommandParameter(
        Name = "misplaced",
        Description = "🟨 Exists but wrong position, ? = unknown (e.g. ??t?? = T isn't 3rd)",
        MaxLength = 5,
        MinLength = 0)]
    string misplacedLetters = "",
    [SlashCommandParameter(
        Name = "exclude",
        Description = "⬛ Not in the word (e.g. abc = no A, B, or C)",
        MaxLength = 26,
        MinLength = 0)]
    string excludeLetters = "")
    {
        LogReceivedInteraction(
            logger,
            Context.Interaction.Data.Name,
            Context.User.Username,
            Context.User.Id);

        var wizdleRequest = new WizdleRequest();
        if (!string.IsNullOrWhiteSpace(correctLetters))
        {
            wizdleRequest.CorrectLetters = correctLetters.Replace(Environment.NewLine, string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        if (!string.IsNullOrWhiteSpace(misplacedLetters))
        {
            wizdleRequest.MisplacedLetters = misplacedLetters.Replace(Environment.NewLine, string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        if (!string.IsNullOrWhiteSpace(excludeLetters))
        {
            wizdleRequest.ExcludeLetters = excludeLetters.Replace(Environment.NewLine, string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        WizdleResponse wizdleResponse = await wizdleApiClient.PostWizdleRequestAsync(wizdleRequest);

        var wordsList = wizdleResponse.Words.ToList();

        if (wordsList.Count == 0)
        {
            return $"⚠️ **No words found**{Environment.NewLine}" +
                string.Join(Environment.NewLine, wizdleResponse.Messages.Select(m => $"> {m}"));
        }

        bool isTruncated = wordsList.Count > 600;
        IEnumerable<string> displayWords = isTruncated ? wordsList.Take(600) : wordsList;

        string header = isTruncated
            ? $"### 🟩 {wordsList.Count} words found *(showing first 600)*{Environment.NewLine}"
            : $"### 🟩 {wordsList.Count} word(s) found:{Environment.NewLine}";

        return header + string.Join(", ", displayWords);
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
