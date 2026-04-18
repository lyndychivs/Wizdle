namespace Wizdle.Web.Functional.Tests.Pages;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Playwright;

internal sealed class HomePage : BasePage
{
    private const string PageTitle = "Wizdle | Solve Wordle...";

    private const string TextBoxCorretLetter1 = "Correct Letter 1";
    private const string TextBoxCorretLetter2 = "Correct Letter 2";
    private const string TextBoxCorretLetter3 = "Correct Letter 3";
    private const string TextBoxCorretLetter4 = "Correct Letter 4";
    private const string TextBoxCorretLetter5 = "Correct Letter 5";

    private const string TextBoxMisplacedLetter1 = "Misplaced Letter 1";
    private const string TextBoxMisplacedLetter2 = "Misplaced Letter 2";
    private const string TextBoxMisplacedLetter3 = "Misplaced Letter 3";
    private const string TextBoxMisplacedLetter4 = "Misplaced Letter 4";
    private const string TextBoxMisplacedLetter5 = "Misplaced Letter 5";

    private const string TextBoxExcludedLetters = "Excluded Letters";

    private const string ButtonSearch = "Search";

    private const string ImageWizdleLogo = "Wizdle";

    public HomePage(IPage page)
        : base(page, PageTitle)
    {
    }

    public async Task<bool> IsCorrectLetter1Visible()
    {
        return await IsTextBoxVisible(TextBoxCorretLetter1).ConfigureAwait(false);
    }

    public async Task<bool> IsCorrectLetter2Visible()
    {
        return await IsTextBoxVisible(TextBoxCorretLetter2).ConfigureAwait(false);
    }

    public async Task<bool> IsCorrectLetter3Visible()
    {
        return await IsTextBoxVisible(TextBoxCorretLetter3).ConfigureAwait(false);
    }

    public async Task<bool> IsCorrectLetter4Visible()
    {
        return await IsTextBoxVisible(TextBoxCorretLetter4).ConfigureAwait(false);
    }

    public async Task<bool> IsCorrectLetter5Visible()
    {
        return await IsTextBoxVisible(TextBoxCorretLetter5).ConfigureAwait(false);
    }

    public async Task<bool> IsMisplacedLetter1Visible()
    {
        return await IsTextBoxVisible(TextBoxMisplacedLetter1).ConfigureAwait(false);
    }

    public async Task<bool> IsMisplacedLetter2Visible()
    {
        return await IsTextBoxVisible(TextBoxMisplacedLetter2).ConfigureAwait(false);
    }

    public async Task<bool> IsMisplacedLetter3Visible()
    {
        return await IsTextBoxVisible(TextBoxMisplacedLetter3).ConfigureAwait(false);
    }

    public async Task<bool> IsMisplacedLetter4Visible()
    {
        return await IsTextBoxVisible(TextBoxMisplacedLetter4).ConfigureAwait(false);
    }

    public async Task<bool> IsMisplacedLetter5Visible()
    {
        return await IsTextBoxVisible(TextBoxMisplacedLetter5).ConfigureAwait(false);
    }

    public async Task<bool> IsExcludedLettersVisible()
    {
        return await IsTextBoxVisible(TextBoxExcludedLetters).ConfigureAwait(false);
    }

    public async Task<bool> IsSearchButtonVisible()
    {
        return await IsButtonVisibile(ButtonSearch).ConfigureAwait(false);
    }

    public async Task<bool> IsWizdleLogoVisible()
    {
        return await IsImageVisible(ImageWizdleLogo).ConfigureAwait(false);
    }

    public async Task ClickSearchButton()
    {
        await ClickButton(ButtonSearch).ConfigureAwait(false);
    }

    public async Task SetCorrectLetter1(char letter)
    {
        await SetTextBox(TextBoxCorretLetter1, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetCorrectLetter2(char letter)
    {
        await SetTextBox(TextBoxCorretLetter2, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetCorrectLetter3(char letter)
    {
        await SetTextBox(TextBoxCorretLetter3, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetCorrectLetter4(char letter)
    {
        await SetTextBox(TextBoxCorretLetter4, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetCorrectLetter5(char letter)
    {
        await SetTextBox(TextBoxCorretLetter5, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetMisplacedLetter1(char letter)
    {
        await SetTextBox(TextBoxMisplacedLetter1, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetMisplacedLetter2(char letter)
    {
        await SetTextBox(TextBoxMisplacedLetter2, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetMisplacedLetter3(char letter)
    {
        await SetTextBox(TextBoxMisplacedLetter3, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetMisplacedLetter4(char letter)
    {
        await SetTextBox(TextBoxMisplacedLetter4, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetMisplacedLetter5(char letter)
    {
        await SetTextBox(TextBoxMisplacedLetter5, letter.ToString()).ConfigureAwait(false);
    }

    public async Task SetExcludedLetters(string letters)
    {
        await SetTextBox(TextBoxExcludedLetters, letters).ConfigureAwait(false);
    }

    public async Task<IEnumerable<string>> GetPossibleWords()
    {
        await WaitForNetworkIdle().ConfigureAwait(false);

        ILocator wordLocator = Page.GetByLabel("Word");

        try
        {
            await wordLocator.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = VisibilityTimeout,
            }).ConfigureAwait(false);
        }
        catch (TimeoutException)
        {
            return [];
        }

        int count = await wordLocator.CountAsync().ConfigureAwait(false);

        var words = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            try
            {
                string? wordText = await wordLocator.Nth(i).TextContentAsync(new LocatorTextContentOptions()
                {
                    Timeout = VisibilityTimeout,
                }).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(wordText))
                {
                    continue;
                }

                words.Add(wordText);
            }
            catch (TimeoutException)
            {
                return words;
            }
        }

        return words;
    }

    public async Task<bool> IsPossibleWordsVisible()
    {
        await WaitForNetworkIdle().ConfigureAwait(false);

        return await DoesPageContainText("Possible Words:").ConfigureAwait(false);
    }
}
