namespace Wizdle.Performance.Tests;

using BenchmarkDotNet.Running;

internal sealed class Program
{
    private static void Main()
    {
        _ = BenchmarkRunner.Run<WizdleEngineTests>();
    }
}
