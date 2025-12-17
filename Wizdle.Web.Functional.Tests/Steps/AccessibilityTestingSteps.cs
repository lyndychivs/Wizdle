namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;

using Microsoft.Playwright;

using NUnit.Framework;

using Reqnroll;

[Binding]
internal sealed class AccessibilityTestingSteps
{
    private readonly IReqnrollOutputHelper _reqnrollOutputHelper;

    private readonly IPage _page;

    private AxeResult? _axeResult;

    public AccessibilityTestingSteps(IReqnrollOutputHelper reqnrollOutputHelper, IPage page)
    {
        _reqnrollOutputHelper = reqnrollOutputHelper ?? throw new ArgumentNullException(nameof(reqnrollOutputHelper));
        _page = page ?? throw new ArgumentNullException(nameof(page));
    }

    [StepDefinition("I perform an Accessibility audit on the current Page")]
    public async Task PerformAccessibilityAudit()
    {
        _axeResult = await _page.RunAxe(GetAxeRunOptions());

        if (_axeResult is null)
        {
            throw new InvalidOperationException("Axe Result is null after running audit.");
        }

        _reqnrollOutputHelper.WriteLine($"axe-core ran against {_axeResult.Url} on {_axeResult.Timestamp}");

        _reqnrollOutputHelper.WriteLine($"axe-core found {_axeResult.Violations.Length} Violations:");
        foreach (AxeResultItem violation in _axeResult.Violations)
        {
            _reqnrollOutputHelper.WriteLine($"- Rule Id: {violation.Id} Description: {violation.Description} Impact: {violation.Impact} HelpUrl: {violation.HelpUrl}");

            foreach (AxeResultNode node in violation.Nodes)
            {
                _reqnrollOutputHelper.WriteLine($"\tViolation found at: {node.Target}");
                _reqnrollOutputHelper.WriteLine($"\t...with HTML: {node.Html}");
            }
        }
    }

    [StepDefinition("the Page should have no Accessibility Violations")]
    public async Task AssertNoAccessibilityViolations()
    {
        if (_axeResult is null)
        {
            throw new InvalidOperationException("Axe Result is null. Ensure that an accessibility audit has been performed before asserting on results.");
        }

        Assert.That(
            _axeResult.Violations,
            Is.Empty,
            $"Expected no Accessibility Violations, but found {_axeResult.Violations.Length} Violations.");
    }

    private static AxeRunOptions GetAxeRunOptions()
    {
        var axeRunOptions = new AxeRunOptions();

        axeRunOptions.Rules ??= [];

        if (axeRunOptions.Rules.TryGetValue("color-contrast", out RuleOptions? ruleOptions))
        {
            ruleOptions.Enabled = false;
        }
        else
        {
            axeRunOptions.Rules["color-contrast"] = new RuleOptions() { Enabled = false };
        }

        return axeRunOptions;
    }
}
