namespace Wizdle.Tests.Words
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Wizdle.Words;

    [TestFixture]
    public class WordsTests
    {
        private readonly Words _words;

        public WordsTests()
        {
            _words = new Words();
        }

        [Test]
        public void GetWords_WhenCalled_ReturnsNonEmptyCollection()
        {
            IEnumerable<string> result = _words.GetWords();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void GetWords_WhenCalled_ReturnsAKnownWord()
        {
            IEnumerable<string> result = _words.GetWords();

            Assert.That(result, Does.Contain("apple"));
        }

        [Test]
        public void GetWords_WhenCalled_ReturnsOnlyLowercaseWords()
        {
            IEnumerable<string> result = _words.GetWords();

            Assert.That(result, Is.All.Matches<string>(w => w.Equals(w, StringComparison.CurrentCulture)));
        }

        [Test]
        public void GetWords_WhenCalled_ReturnsOnlyFiveLetterWords()
        {
            IEnumerable<string> result = _words.GetWords();

            Assert.That(result, Is.All.Matches<string>(w => w.Length == 5));
        }

        [Test]
        public void GetWords_WhenCalled_ReturnsNoDuplicateWords()
        {
            IEnumerable<string> result = _words.GetWords();

            Assert.That(result.Count(), Is.EqualTo(result.Distinct().Count()));
        }
    }
}