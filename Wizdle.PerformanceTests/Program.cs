using BenchmarkDotNet.Running;

using Wizdle.PerformanceTests;

internal class Program
{
    private static void Main()
    {
        _ = BenchmarkRunner.Run<WizdleEngineTests>();
    }
}