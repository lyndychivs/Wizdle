namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using Reqnroll;

using Wizdle.Web.Functional.Tests.Models;
using Wizdle.Web.Functional.Tests.Pages;

[Binding]
internal sealed class HomePageSteps
{
    private readonly HomePage _homePage;

    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    public HomePageSteps(HomePage homePage, IReqnrollOutputHelper reqnrollOutputHelper)
    {
        _homePage = homePage ?? throw new ArgumentNullException(nameof(homePage));
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
    }

    [StepDefinition("the Home page should display all expected elements")]
    public async Task AssertTheHomePageDisplaysAllExpectedElements()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _homePage.IsWizdleLogoVisible(), Is.True, "Wizdle Logo is not visible.");

            Assert.That(await _homePage.IsSearchButtonVisible(), Is.True, "Search Button is not visible.");

            Assert.That(await _homePage.IsCorrectLetter1Visible(), Is.True, "Correct Letter 1 TextBox is not visible.");
            Assert.That(await _homePage.IsCorrectLetter2Visible(), Is.True, "Correct Letter 2 TextBox is not visible.");
            Assert.That(await _homePage.IsCorrectLetter3Visible(), Is.True, "Correct Letter 3 TextBox is not visible.");
            Assert.That(await _homePage.IsCorrectLetter4Visible(), Is.True, "Correct Letter 4 TextBox is not visible.");
            Assert.That(await _homePage.IsCorrectLetter5Visible(), Is.True, "Correct Letter 5 TextBox is not visible.");

            Assert.That(await _homePage.IsMisplacedLetter1Visible(), Is.True, "Misplaced Letter 1 TextBox is not visible.");
            Assert.That(await _homePage.IsMisplacedLetter2Visible(), Is.True, "Misplaced Letter 2 TextBox is not visible.");
            Assert.That(await _homePage.IsMisplacedLetter3Visible(), Is.True, "Misplaced Letter 3 TextBox is not visible.");
            Assert.That(await _homePage.IsMisplacedLetter4Visible(), Is.True, "Misplaced Letter 4 TextBox is not visible.");
            Assert.That(await _homePage.IsMisplacedLetter5Visible(), Is.True, "Misplaced Letter 5 TextBox is not visible.");

            Assert.That(await _homePage.IsExcludedLettersVisible(), Is.True, "Excluded Letters TextBox is not visible.");

            Assert.That(await _homePage.IsDarkmodeButtonVisible(), Is.True, "Darkmode Button is not visible.");
        }
    }

    [StepDefinition("on the Home page, I click on the Dark Mode button")]
    public async Task ClickDarkModeButton()
    {
        await _homePage.ClickDarkModeButton();
    }

    [StepDefinition("the Home page theme is using {PageTheme} colors")]
    public async Task AssertPageThemeColorsAre(PageTheme themeName)
    {
        switch (themeName)
        {
            case PageTheme.Default:
                Assert.That(
                    await _homePage.IsDefaultBackgroundColor(),
                    Is.True,
                    $"Expected Page to use {themeName} background color, but it does not.");
                break;

            case PageTheme.DarkMode:
                Assert.That(
                    await _homePage.IsDarkmodeEnabled(),
                    Is.True,
                    $"Expected Page to use {themeName} background color, but it does not.");
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(themeName));
        }
    }

    [StepDefinition("on the Home page, I click on the Search button")]
    public async Task ClickSearchButton()
    {
        await _homePage.ClickSearchButton();
    }

    [StepDefinition("on the Home page, I specify the Correct Letters as")]
    public async Task SetCorrectLettersAs(DataTable dataTable)
    {
        Letters letters = dataTable.CreateInstance<Letters>();

        await _homePage.SetCorrectLetter1(letters.Letter1);
        await _homePage.SetCorrectLetter2(letters.Letter2);
        await _homePage.SetCorrectLetter3(letters.Letter3);
        await _homePage.SetCorrectLetter4(letters.Letter4);
        await _homePage.SetCorrectLetter5(letters.Letter5);
    }

    [StepDefinition("on the Home page, I specify the Misplaced Letters as")]
    public async Task SetMisplacedLettersAs(DataTable dataTable)
    {
        Letters letters = dataTable.CreateInstance<Letters>();

        await _homePage.SetMisplacedLetter1(letters.Letter1);
        await _homePage.SetMisplacedLetter2(letters.Letter2);
        await _homePage.SetMisplacedLetter3(letters.Letter3);
        await _homePage.SetMisplacedLetter4(letters.Letter4);
        await _homePage.SetMisplacedLetter5(letters.Letter5);
    }

    [StepDefinition("on the Home page, I specify the Excluded Letters as {string}")]
    public async Task SetExcludedLettersAs(string letters)
    {
        await _homePage.SetExcludedLetters(letters);
    }

    [StepDefinition("on the Home page, the Possible Words should display only one word, {string}")]
    public async Task AssertPossibleWordsOnlyContains(string expectedWord)
    {
        var expectedWords = new List<string> { expectedWord };
        IEnumerable<string> actualWords = await _homePage.GetPossibleWords();

        await TakeScreenshot();

        Assert.That(
            actualWords,
            Is.EqualTo(expectedWords),
            $"Possible Words returned does not match, expected to find only \"{expectedWord}\" but was \"{string.Join(", ", actualWords)}\".");
    }

    [StepDefinition("on the Home page, the Possible Words should display multiple words")]
    public async Task AssertPossibleWordsContainsMultipleWords()
    {
        IEnumerable<string> actualWords = await _homePage.GetPossibleWords();

        await TakeScreenshot();

        Assert.That(
            actualWords.Count(),
            Is.GreaterThan(1),
            $"Expected Possible Words to contain multiple words, but found only \"{string.Join(", ", actualWords)}\".");
        _reqnrollOutputHelper.WriteLine($"Possible Words:{Environment.NewLine}{string.Join(Environment.NewLine, actualWords)}");
    }

    [StepDefinition("on the Home page, no Possible Words should be displayed")]
    public async Task AssertNoPossibleWordsAreDisplayed()
    {
        await TakeScreenshot();

        const string PossibleWordsTitleText = "Possible Words:";
        using (Assert.EnterMultipleScope())
        {
            Assert.That(await _homePage.DoesPageContainText(PossibleWordsTitleText), Is.False, $"Expected Title (\"{PossibleWordsTitleText}\") to not be displayed, but is visible.");
            Assert.That(await _homePage.IsPossibleWordsVisible(), Is.False, "Expected no Possible Words to be displayed, but some are visible.");
        }
    }

    private async Task TakeScreenshot()
    {
        string screenshotPath = await _homePage.GetScreenshot();
        _reqnrollOutputHelper.AddAttachment(screenshotPath);
    }
}
