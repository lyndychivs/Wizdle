using NUnit.Framework;

[assembly: FixtureLifeCycle(LifeCycle.SingleInstance)]
[assembly: Parallelizable(ParallelScope.All)]
