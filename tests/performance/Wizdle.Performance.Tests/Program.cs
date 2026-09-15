namespace Wizdle.Performance.Tests;

using BenchmarkDotNet.Running;

internal static class Program
{
    private static void Main()
    {
        _ = BenchmarkRunner.Run<WizdleEngineTests>();
    }
}
