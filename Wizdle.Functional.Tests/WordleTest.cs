namespace Wizdle.Functional.Tests;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

using Wizdle.Functional.Tests.Logging;
using Wizdle.Functional.Tests.Models;
using Wizdle.Models;

[TestFixture]
public class WordleTest : PageTest
{
    private const string WordleUrl = "https://www.nytimes.com/games/wordle/index.html";

    private const string WordleTitle = "Wordle — The New York Times";

    private const int MaxAttempts = 6;

    private readonly List<Word> _words;
    private readonly List<char> _correctLetters;
    private readonly List<char> _misplacedLetters;
    private readonly StringBuilder _excludeLetters;

    private readonly WizdleEngine _wizdleEngine;

    private readonly ILogger _logger;

    public WordleTest()
    {
        /*
         * For a one seed strategy the best word is "tales".
         * Using this word leads to success in over 95% of games with an average game length of 3.66 rounds.
         * For a two seed word strategy; start with "cones" and follow with "trial".
         * They lead to success for just over 96% of target words with an average game length of 3.68 rounds.
         * For a three seed word strategy; start with "hates", follow with "round", follow with "climb".
         * This approach leads to a slightly higher success rate of 97%, but with an average game length of 4.2 rounds.
         */
        _words = [new Word("hates"), new Word("round"), new Word("climb")];

        _correctLetters = ['?', '?', '?', '?', '?'];
        _misplacedLetters = ['?', '?', '?', '?', '?'];
        _excludeLetters = new StringBuilder();

        _logger = Logger.CreateConsoleLogger<WordleTest>();
        _wizdleEngine = new WizdleEngine(_logger);
    }

    [SetUp]
    public async Task SetUp()
    {
        await Page.Context.Tracing.StartAsync(new TracingStartOptions()
        {
            Title = TestContext.CurrentContext.Test.FullName,
            Screenshots = true,
            Snapshots = true,
            Sources = true,
            Name = TestContext.CurrentContext.Test.FullName,
        });
    }

    [TearDown]
    public async Task TearDown()
    {
        bool isFailed = TestContext.CurrentContext.Result.Outcome == ResultState.Error
        || TestContext.CurrentContext.Result.Outcome == ResultState.Failure;

        string tracingFilePath = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "playwright-traces",
            $"{Guid.NewGuid()}.zip");

        await Page.Context.Tracing.StopAsync(new TracingStopOptions()
        {
            Path = isFailed ? tracingFilePath : null,
        });

        if (isFailed)
        {
            TestContext.AddTestAttachment(tracingFilePath, "Playwright Trace");
        }
    }

    [Test]
    public async Task TrySolve()
    {
        await Page.GotoAsync(WordleUrl, new PageGotoOptions()
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60_000,
        });

        await Expect(Page).ToHaveTitleAsync(WordleTitle);

        await ClickButtonIfPresent("Reject All");

        await Page.GetByTestId("Play").ClickAsync();

        await ClickButtonIfPresent("Close");

        for (int attempts = 0; attempts < MaxAttempts; attempts++)
        {
            await SubmitWordOnPage(_words[attempts]);

            if (await IsGameOver())
            {
                _logger.LogInformation($"Game ended after {attempts + 1} attempt(s).");
                break;
            }

            await UpdateWordStatusFromPage(_words[attempts]);

            UpdateWizdleRequestData(_words[attempts]);

            if (attempts < 2)
            {
                continue;
            }

            _words.Add(GetNewWordFromWizdle());
        }
    }

    private static async Task<LetterStatus> GetLetterStatusFromElement(ILocator letterElement)
    {
        string accessibleName = await letterElement.GetAttributeAsync("aria-label")
            ?? throw new ArgumentNullException(
                nameof(letterElement),
                $"Failed to get {nameof(LetterStatus)} from {nameof(letterElement)}");

        if (accessibleName.Contains("correct", StringComparison.OrdinalIgnoreCase))
        {
            return LetterStatus.Correct;
        }

        if (accessibleName.Contains("present in", StringComparison.OrdinalIgnoreCase))
        {
            return LetterStatus.Present;
        }

        return LetterStatus.Absent;
    }

    private Word GetNewWordFromWizdle()
    {
        var wizdleRequest = new WizdleRequest
        {
            CorrectLetters = string.Join(string.Empty, _correctLetters),
            MisplacedLetters = string.Join(string.Empty, _misplacedLetters),
            ExcludeLetters = _excludeLetters.ToString(),
        };

        WizdleResponse wizdleResponse = _wizdleEngine.ProcessWizdleRequest(wizdleRequest);

        if (!wizdleResponse.Words.Any())
        {
            Assert.Fail("Wizdle returned no possible words.");
        }

        _logger.LogInformation("Wizdle Suggestions:");
        _logger.LogInformation(string.Join(", ", wizdleResponse.Words));

        if (wizdleResponse.Words.Count() == 1)
        {
            return new Word(wizdleResponse.Words.First());
        }

        int index = new Random().Next(0, wizdleResponse.Words.Count() - 1);
        return new Word(wizdleResponse.Words.ElementAt(index));
    }

    private void UpdateWizdleRequestData(Word word)
    {
        for (int i = 0; i < 5; i++)
        {
            char letter = word.GetCharUpper(i);
            LetterStatus status = word.GetLetterStatus(i);
            switch (status)
            {
                case LetterStatus.Correct:
                    _correctLetters[i] = letter;
                    break;

                case LetterStatus.Present:
                    _misplacedLetters[i] = letter;
                    break;

                case LetterStatus.Absent:
                    _excludeLetters.Append(letter);
                    break;
            }
        }
    }

    private async Task SubmitWordOnPage(Word word)
    {
        _logger.LogInformation($"Submit: {word}");

        foreach (char letter in word.ToString())
        {
            char lowLetter = char.ToLower(letter, CultureInfo.InvariantCulture);

            if (await ClickButtonIfPresent($"add {lowLetter}"))
            {
                continue;
            }

            if (await ClickButtonIfPresent($"{lowLetter} present"))
            {
                continue;
            }

            if (await ClickButtonIfPresent($"{lowLetter} absent"))
            {
                continue;
            }

            await Page.GetByRole(AriaRole.Button, new() { Name = $"{lowLetter} correct" }).ClickAsync();
        }

        await Page.GetByRole(AriaRole.Button, new() { Name = "enter" }).ClickAsync();

        await Page.WaitForTimeoutAsync(10_000);
    }

    private async Task UpdateWordStatusFromPage(Word word)
    {
        word.SetLetterStatus(0, await GetLetterStatusFromImage("1st", word.GetCharUpper(0)));
        word.SetLetterStatus(1, await GetLetterStatusFromImage("2nd", word.GetCharUpper(1)));
        word.SetLetterStatus(2, await GetLetterStatusFromImage("3rd", word.GetCharUpper(2)));
        word.SetLetterStatus(3, await GetLetterStatusFromImage("4th", word.GetCharUpper(3)));
        word.SetLetterStatus(4, await GetLetterStatusFromImage("5th", word.GetCharUpper(4)));
    }

    private async Task<LetterStatus> GetLetterStatusFromImage(string position, char letter)
    {
        ILocator fifthLetterElement = Page.GetByRole(AriaRole.Img, new()
        {
            Name = $"{position} letter, {letter},",
        }).Nth(0);

        await Expect(fifthLetterElement).ToBeVisibleAsync();

        return await GetLetterStatusFromElement(fifthLetterElement);
    }

    private async Task<bool> ClickButtonIfPresent(string buttonName)
    {
        var buttonLocator = Page.GetByRole(AriaRole.Button, new() { Name = buttonName });

        try
        {
            await buttonLocator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5_000,
            });

            if (!await buttonLocator.IsVisibleAsync())
            {
                return false;
            }

            await buttonLocator.ClickAsync();
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    private async Task<bool> IsGameOver()
    {
        if (await ClickButtonIfPresent("Close"))
        {
            var congratsHeading = Page.GetByRole(AriaRole.Heading, new() { Name = "Congratulations!" });

            try
            {
                await congratsHeading.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 5_000,
                });

                if (await congratsHeading.IsVisibleAsync())
                {
                    _logger.LogInformation($"Solved Wordle!");
                    return true;
                }
            }
            catch (TimeoutException)
            {
                Assert.Fail($"Failed to solve Wordle!");
            }
        }

        return false;
    }
}
