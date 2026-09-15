using NUnit.Framework;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
[assembly: Parallelizable(ParallelScope.None)]
[assembly: LevelOfParallelism(1)]
