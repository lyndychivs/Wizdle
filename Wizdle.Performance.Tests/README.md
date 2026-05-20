# ⏱️ Wizdle.Performance.Tests

[![Performance Tests](https://github.com/lyndychivs/Wizdle/actions/workflows/performance_tests.yaml/badge.svg?branch=main)](https://github.com/lyndychivs/Wizdle/actions/workflows/performance_tests.yaml)

Performance benchmarks for the Wizdle engine using [BenchmarkDotNet](https://benchmarkdotnet.org/).

## Overview

Benchmarks are parameterized by word length (1–5 characters) and cover:

- **WizdleEngine_WithOnlyCorrectLetters** — filtering with only correct letter positions
- **WizdleEngine_WithOnlyMisplacedLetters** — filtering with only misplaced letters

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

Results include mean/median/stddev, memory allocations, and statistical analysis. Stored in `BenchmarkDotNet.Artifacts/` after each run and published to the workflow summary.

## CI/CD

Performance tests run on every push, weekly on Sundays at 4 AM UTC, and as part of the release workflow.

