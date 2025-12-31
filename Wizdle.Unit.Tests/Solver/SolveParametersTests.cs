namespace Wizdle.Unit.Tests.Solver;

using NUnit.Framework;

using Wizdle.Solver;

[TestFixture]
public class SolveParametersTests
{
    [Test]
    public void Constructor_WithNoParameters_ReturnsDefaultSolveParameters()
    {
        var result = new SolveParameters();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.Empty);
            Assert.That(result.MisplacedLetters, Is.Empty);
            Assert.That(result.ExcludeLetters, Is.Empty);
        }
    }

    [Test]
    public void Constructor_WithParameters_ReturnsSolveParametersContainingParameters()
    {
        var result = new SolveParameters
        {
            CorrectLetters = ['a'],
            MisplacedLetters = ['b'],
            ExcludeLetters = ['c'],
        };

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.CorrectLetters, Is.EqualTo(['a']));
            Assert.That(result.MisplacedLetters, Is.EqualTo(['b']));
            Assert.That(result.ExcludeLetters, Is.EqualTo(['c']));
        }
    }

    [Test]
    public void ToString_WithSingleParameters_ReturnsParametersInString()
    {
        var result = new SolveParameters
        {
            CorrectLetters = ['a'],
            MisplacedLetters = ['b'],
            ExcludeLetters = ['c'],
        };

        Assert.That(result.ToString(), Is.EqualTo("CorrectLetters: \"a\"       MisplacedLetters: \"b\"     ExcludeLetters: \"c\""));
    }

    [Test]
    public void ToString_WithMultipleParameters_ReturnsParametersInString()
    {
        var result = new SolveParameters
        {
            CorrectLetters = ['a', 'a'],
            MisplacedLetters = ['b', 'b'],
            ExcludeLetters = ['c', 'c'],
        };

        Assert.That(result.ToString(), Is.EqualTo("CorrectLetters: \"aa\"      MisplacedLetters: \"bb\"    ExcludeLetters: \"cc\""));
    }

    [Test]
    public void ToString_WithNoParameters_ReturnsEmptyValuesInString()
    {
        var result = new SolveParameters
        {
            CorrectLetters = [],
            MisplacedLetters = [],
            ExcludeLetters = [],
        };

        Assert.That(result.ToString(), Is.EqualTo("CorrectLetters: \"\"        MisplacedLetters: \"\"      ExcludeLetters: \"\""));
    }
}
