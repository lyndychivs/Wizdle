# ⏱️ Wizdle.Performance.Tests

[![Performance Tests](https://github.com/lyndychivs/Wizdle/actions/workflows/performance_tests.yaml/badge.svg?branch=main)](https://github.com/lyndychivs/Wizdle/actions/workflows/performance_tests.yaml)

Performance benchmarks for the Wizdle engine using [BenchmarkDotNet](https://benchmarkdotnet.org/).

## Overview

This project contains performance tests that measure the execution time and efficiency of the `WizdleEngine` under various scenarios. Benchmarks are run against different word lengths and input configurations to track performance characteristics over time.

The benchmarks test:

- **WizdleEngine_WithOnlyCorrectLetters** — Performance when filtering with only correct letter positions
- **WizdleEngine_WithOnlyMisplacedLetters** — Performance when filtering with only misplaced letters

Each benchmark is parameterized to run with word lengths from 1 to 5 characters.

## Running Locally

Run the performance tests using the `make` command:

```sh
make perf
```

Or directly via .NET CLI:

```sh
dotnet run --project Wizdle.Performance.Tests/Wizdle.Performance.Tests.csproj --configuration Release
```

## Results

Benchmark results are published to the GitHub Actions workflow summary and archived as artifacts. Results include:

- Execution time (mean, median, standard deviation)
- Memory allocations
- Statistical analysis

Results are stored in `BenchmarkDotNet.Artifacts/` after each run.

## CI/CD

Performance tests run automatically:

- On every push
- On a weekly schedule (Sundays at 4 AM UTC)
- As part of the release workflow

Results are tracked over time to detect performance regressions.

