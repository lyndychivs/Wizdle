namespace Wizdle.Discord;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NetCord.Services.ApplicationCommands;

using Wizdle.Models;

public partial class WordSlashCommand(ILogger<WordSlashCommand> logger, WizdleApiClient wizdleApiClient)
    : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("word", "Search for possible Wordle Words")]
    public async Task<string> GetWordsAsync(
    [SlashCommandParameter(
        Name = "correct",
        Description = "The correct letters known to exist in the Word (Example second letter correct is 'R' = \"?R\")",
        MaxLength = 5,
        MinLength = 0)]
    string correctLetters = "",
    [SlashCommandParameter(
        Name = "misplaced",
        Description = "The misplaced letters known to exist in the Word (Example third letter misplaced is 'T' = \"??T\")",
        MaxLength = 5,
        MinLength = 0)]
    string misplacedLetters = "",
    [SlashCommandParameter(
        Name = "exclude",
        Description = "The letters that are known to not exist in the Word (Example: \"ABC\")",
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
            wizdleRequest.CorrectLetters = correctLetters.Replace(Environment.NewLine, string.Empty);
        }

        if (!string.IsNullOrWhiteSpace(misplacedLetters))
        {
            wizdleRequest.MisplacedLetters = misplacedLetters.Replace(Environment.NewLine, string.Empty);
        }

        if (!string.IsNullOrWhiteSpace(excludeLetters))
        {
            wizdleRequest.ExcludeLetters = excludeLetters.Replace(Environment.NewLine, string.Empty);
        }

        WizdleResponse wizdleResponse = await wizdleApiClient.PostWizdleRequestAsync(wizdleRequest);

        IEnumerable<string> response = wizdleResponse.Words;
        if (wizdleResponse.Words.Count() > 600)
        {
            response = wizdleResponse.Words.Take(600);
        }

        if (!wizdleResponse.Words.Any())
        {
            response = wizdleResponse.Messages;
        }

        return string.Join(", ", response);
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
