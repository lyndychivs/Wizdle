namespace Wizdle.Web.Unit.Tests;

using System;
using System.Collections.Generic;
using System.Net.Http;

using AngleSharp.Dom;

using Bunit;

using Microsoft.Extensions.DependencyInjection;

using MudBlazor.Services;

using NUnit.Framework;

using Wizdle.Models;
using Wizdle.Web;
using Wizdle.Web.Components.Pages;

using TestContext = Bunit.TestContext;

[TestFixture]
public sealed class HomeTests
{
    private TestContext _testContext = default!;

    [SetUp]
    public void SetUp()
    {
        _testContext = new TestContext()
        {
            JSInterop =
            {
                Mode = JSRuntimeMode.Loose,
            },
        };
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    [Test]
    public void Home_Renders_CorrectLettersHeading()
    {
        RegisterServices();

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        Assert.That(cut.Markup, Does.Contain("Correct Letters"));
    }

    [Test]
    public void Home_Renders_MisplacedLettersHeading()
    {
        RegisterServices();

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        Assert.That(cut.Markup, Does.Contain("Misplaced Letters"));
    }

    [Test]
    public void Home_Renders_ExcludedLettersHeading()
    {
        RegisterServices();

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        Assert.That(cut.Markup, Does.Contain("Excluded Letters"));
    }

    [Test]
    public void Home_Renders_SearchButton()
    {
        RegisterServices();

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        Assert.That(cut.Find("#btnSearch"), Is.Not.Null);
    }

    [Test]
    public void Home_InitialState_HasNoWords()
    {
        RegisterServices();

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        Assert.That(cut.FindAll("[aria-label='Word']"), Is.Empty);
    }

    [Test]
    public void Home_InitialState_HasNoIntroText()
    {
        RegisterServices();

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        Assert.That(cut.Find("[aria-label='Response Title']").TextContent, Is.Empty);
    }

    [Test]
    public void Home_SolveWithMatchingWords_DisplaysWords()
    {
        WizdleResponse response = new()
        {
            Words = ["crane", "trace"],
        };
        RegisterServices(response);

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        cut.Find("#btnSearch").Click();

        cut.WaitForState(() => cut.FindAll("[aria-label='Word']").Count == 2);

        IReadOnlyList<IElement> words = cut.FindAll("[aria-label='Word']");
        Assert.That(words, Has.Count.EqualTo(2));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(words[0].TextContent, Is.EqualTo("CRANE"));
            Assert.That(words[1].TextContent, Is.EqualTo("TRACE"));
        }
    }

    [Test]
    public void Home_SolveWithMatchingWords_ShowsPossibleWordsIntro()
    {
        WizdleResponse response = new()
        {
            Words = ["crane"],
        };
        RegisterServices(response);

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        cut.Find("#btnSearch").Click();

        cut.WaitForState(() => string.Equals(cut.Find("[aria-label='Response Title']").TextContent, "Possible Words:", StringComparison.Ordinal));

        Assert.That(cut.Find("[aria-label='Response Title']").TextContent, Is.EqualTo("Possible Words:"));
    }

    [Test]
    public void Home_SolveWithNoMatchingWords_HasNoWordsDisplayed()
    {
        WizdleResponse response = new()
        {
            Words = [],
        };
        FakeHttpMessageHandler handler = RegisterServices(response);

        IRenderedComponent<Home> cut = _testContext.RenderComponent<Home>();

        cut.Find("#btnSearch").Click();

        cut.WaitForState(() => handler.RequestCount == 1);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(cut.FindAll("[aria-label='Word']"), Is.Empty);
            Assert.That(cut.Find("[aria-label='Response Title']").TextContent, Is.Empty);
        }
    }

    private FakeHttpMessageHandler RegisterServices(WizdleResponse? response = null)
    {
        response ??= new WizdleResponse();

        FakeHttpMessageHandler handler = new(response);
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri("http://localhost"),
        };

        _testContext.Services.AddSingleton(new WizdleHttpClient(httpClient));
        _testContext.Services.AddMudServices();

        return handler;
    }
}
