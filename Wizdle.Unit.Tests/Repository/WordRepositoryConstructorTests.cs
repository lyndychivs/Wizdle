namespace Wizdle.Unit.Tests.Repository;

using System;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using Wizdle.Repository;
using Wizdle.Words;

[TestFixture]
public class WordRepositoryConstructorTests
{
    [Test]
    public void Constructor_WithValidParameters_ReturnsWordRepository()
    {
        var loggerMock = new Mock<ILogger>();
        var wordsMock = new Mock<IWords>();

        var result = new WordRepository(loggerMock.Object, wordsMock.Object);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void SingleConstructor_WithValidLogger_ReturnsWordRepository()
    {
        var loggerMock = new Mock<ILogger>();

        var result = new WordRepository(loggerMock.Object);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        var wordsMock = new Mock<IWords>();

        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => new WordRepository(null!, wordsMock.Object));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("logger"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'logger')"));
        }
    }

    [Test]
    public void SingleConstructor_WithNullLogger_ThrowsArgumentNullException()
    {
        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => new WordRepository(null!));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("logger"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'logger')"));
        }
    }

    [Test]
    public void Constructor_WithNullWordFile_ThrowsArgumentNullException()
    {
        var loggerMock = new Mock<ILogger>();

        ArgumentNullException? ex = Assert.Throws<ArgumentNullException>(() => new WordRepository(loggerMock.Object, null!));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ex?.ParamName, Is.EqualTo("words"));
            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'words')"));
        }
    }
}
