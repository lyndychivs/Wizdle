namespace Wizdle.Web.Functional.Tests.Steps;

using System;
using System.Threading.Tasks;

using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;

using Microsoft.Playwright;

internal static class AccessibilityTestingSteps
{
    public static async Task<AxeResult> ExecuteAccessibilityTesting(IPage page)
    {
        AxeResult axeResult = await page.RunAxe();

        Console.WriteLine($"axe-core ran against {axeResult.Url} on {axeResult.Timestamp}");

        if (axeResult.Violations.Length == 0)
        {
            Console.WriteLine("No Accessibility Violations found.");
            return axeResult;
        }

        Console.WriteLine($"axe-core found {axeResult.Violations.Length} Violations:");
        foreach (AxeResultItem violation in axeResult.Violations)
        {
            Console.WriteLine($"- Rule Id: {violation.Id} Description: {violation.Description} Impact: {violation.Impact} HelpUrl: {violation.HelpUrl}");

            foreach (AxeResultNode node in violation.Nodes)
            {
                Console.WriteLine($"\tViolation found at: {node.Target}");
                Console.WriteLine($"\t...with HTML: {node.Html}");
            }
        }

        return axeResult;
    }
}
