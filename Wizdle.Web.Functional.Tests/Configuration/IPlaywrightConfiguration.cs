namespace Wizdle.Web.Functional.Tests.Configuration;

internal interface IPlaywrightConfiguration
{
    string BrowserName { get; }

    bool Headless { get; }

    string Channel { get; }
}
