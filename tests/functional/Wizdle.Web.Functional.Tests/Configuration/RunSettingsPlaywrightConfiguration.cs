namespace Wizdle.Web.Functional.Tests.Configuration;

using System;
using System.IO;
using System.Xml.Linq;

internal sealed class RunSettingsPlaywrightConfiguration : IPlaywrightConfiguration
{
    private const string RunSettingsFileName = ".runsettings";

    public RunSettingsPlaywrightConfiguration()
    {
        string runSettingsPath = Path.Combine(AppContext.BaseDirectory, RunSettingsFileName);

        if (!File.Exists(runSettingsPath))
        {
            throw new FileNotFoundException(
                $"Could not find '{RunSettingsFileName}' in '{AppContext.BaseDirectory}'.");
        }

        XDocument document = XDocument.Load(runSettingsPath);

        XElement playwright = document.Root?.Element("Playwright")
            ?? throw new InvalidOperationException(
                $"The .runsettings file at '{runSettingsPath}' does not contain a <Playwright> section.");

        BrowserName = playwright.Element("BrowserName")?.Value
            ?? throw new InvalidOperationException(
                "The <Playwright> section is missing a <BrowserName> element.");

        XElement launchOptions = playwright.Element("LaunchOptions")
            ?? throw new InvalidOperationException(
                "The <Playwright> section is missing a <LaunchOptions> element.");

        string headlessValue = launchOptions.Element("Headless")?.Value
            ?? throw new InvalidOperationException(
                "The <LaunchOptions> section is missing a <Headless> element.");

        Headless = bool.Parse(headlessValue);

        Channel = launchOptions.Element("Channel")?.Value
            ?? throw new InvalidOperationException(
                "The <LaunchOptions> section is missing a <Channel> element.");
    }

    public string BrowserName { get; }

    public bool Headless { get; }

    public string Channel { get; }

    public override string ToString()
    {
        return $"Browser: {BrowserName}{Environment.NewLine}Channel: {Channel}{Environment.NewLine}Headless: {Headless}";
    }
}
