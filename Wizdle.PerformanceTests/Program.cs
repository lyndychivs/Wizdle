using BenchmarkDotNet.Running;

using Wizdle.PerformanceTests;

internal sealed class Program
{
    private static void Main()
    {
        _ = BenchmarkRunner.Run<WizdleEngineTests>();
    }
}