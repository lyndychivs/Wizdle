namespace Wizdle.Wpf.Unit.Tests;

using System.Windows;

using NUnit.Framework;

[TestFixture]
public class MainWindowTests
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void GetLetterFromInput_WhenInputIsNullOrWhitespace_ReturnsQuestionMark(string? input)
    {
        char result = MainWindow.GetLetterFromInput(input);

        Assert.That(result, Is.EqualTo('?'));
    }

    [TestCase("a", 'a')]
    [TestCase("ab", 'a')]
    public void GetLetterFromInput_WhenInputHasValue_ReturnsFirstCharacter(string input, char expected)
    {
        char result = MainWindow.GetLetterFromInput(input);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void GetVisibility_WhenIsVisibleIsTrue_ReturnsVisible()
    {
        Visibility result = MainWindow.GetVisibility(isVisible: true);

        Assert.That(result, Is.EqualTo(Visibility.Visible));
    }

    [Test]
    public void GetVisibility_WhenIsVisibleIsFalse_ReturnsHidden()
    {
        Visibility result = MainWindow.GetVisibility(isVisible: false);

        Assert.That(result, Is.EqualTo(Visibility.Hidden));
    }
}
