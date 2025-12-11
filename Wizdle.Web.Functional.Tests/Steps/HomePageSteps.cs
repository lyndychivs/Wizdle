namespace Wizdle.Web.Functional.Tests.Steps;

using System;

using Reqnroll;

[Binding]
internal sealed class HomePageSteps
{
    [StepDefinition("the Page title should be {string}")]
    public static void ThenThePageTitleShouldBe(string x)
    {
        Console.WriteLine(x);
    }
}
