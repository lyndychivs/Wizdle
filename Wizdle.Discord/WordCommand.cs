namespace Wizdle.Discord
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using NetCord.Services.ApplicationCommands;

    using Wizdle.Models;

    public class WordCommand(WizdleApiClient wizdleApiClient) : ApplicationCommandModule<ApplicationCommandContext>
    {
        [SlashCommand("word", "Search for possible wordle words")]
        public async Task<string> GetWordsAsync(
        [SlashCommandParameter(Name = "correct", Description = "The correct letters known to exist in the Word, follow the format of `a.b.c` where unknown letters are represented by a dot (`.`)", MaxLength = 5, MinLength = 0)]
        string correctLetters,
        [SlashCommandParameter(Name = "misplaced", Description = "The misplaced letters known to exist in the Word, follow the format of `a.b.c` where unknown letters are represented by a dot (`.`)", MaxLength = 5, MinLength = 0)]
        string misplacedLetters,
        [SlashCommandParameter(Name = "exclude", Description = "The letters that are known to not exist in the Word, follow the format of `abc` where each letter is a single character.", MaxLength = 26, MinLength = 0)]
        string excludeLetters)
        {
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
    }
}