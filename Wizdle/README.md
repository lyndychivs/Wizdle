# Wizdle

[![Build & Test](https://github.com/lyndychivs/Wizdle/actions/workflows/build_test.yaml/badge.svg?branch=main)](https://github.com/lyndychivs/Wizdle/actions/workflows/build_test.yaml)
[![Solve Wordle Today](https://github.com/lyndychivs/Wizdle/actions/workflows/solve_wordle.yaml/badge.svg?branch=main)](https://github.com/lyndychivs/Wizdle/actions/workflows/solve_wordle.yaml)
[![Mutation testing badge](https://img.shields.io/endpoint?style=flat&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2Flyndychivs%2FWizdle%2Fmain)](https://dashboard.stryker-mutator.io/reports/github.com/lyndychivs/Wizdle/main)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Wizdle?style=flat&logo=nuget)](https://www.nuget.org/packages/Wizdle/)

A C# library for solving Wordle.

Wordle is a word puzzle where you guess a 5-letter word in 6 attempts. Each guess reveals which letters are correct, misplaced, or absent. Wizdle takes that feedback and returns a filtered list of candidate words.

**Target Framework:** `netstandard2.0` &nbsp;|&nbsp; **License:** [MIT](https://github.com/lyndychivs/Wizdle/blob/main/LICENSE)

## Install

```sh
dotnet add package Wizdle
```

Available on [NuGet](https://www.nuget.org/packages/Wizdle/) and [GitHub](https://github.com/lyndychivs/Wizdle/pkgs/nuget/Wizdle).

## Prerequisites

| Prerequisite | Note |
| --- | --- |
| `ILogger` implementation | This library requires an implementation of `Microsoft.Extensions.Logging` to construct. (See the `Serilog` example in [Wizdle.Integration.Tests/Logger.cs](https://github.com/lyndychivs/Wizdle/blob/main/Wizdle.Integration.Tests/Logger.cs).) |

The only external dependency is `Microsoft.Extensions.Logging.Abstractions` — the lightweight abstractions package, not the full logging stack.

## Input Format

Each letter property uses a 5-character string. Use `.` as a placeholder for an unknown letter at that position.

| Property | Description | Example |
| --- | --- | --- |
| `CorrectLetters` | Letters at their exact position | `"....t"` — `t` confirmed in position 5 |
| `MisplacedLetters` | Letters present in the word but at the wrong position | `"..rs."` — `r` misplaced in pos 3, `s` misplaced in pos 4 |
| `ExcludeLetters` | Letters confirmed not in the word | `"haebu"` |

## Example

```csharp
using Wizdle;
using Wizdle.Models;

var wizdleEngine = new WizdleEngine(_logger);

var request = new WizdleRequest
{
    CorrectLetters = "....t",
    MisplacedLetters = "..rs.",
    ExcludeLetters = "haebu",
};

WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);
// response.Words: ["skirt", "snort", "sport"]
```

## Response

| Property | Type | Description |
| --- | --- | --- |
| `Words` | `IEnumerable<string>` | Words matching the given criteria |
| `Messages` | `IEnumerable<string>` | Informational messages about the request processed |
