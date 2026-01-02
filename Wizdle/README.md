# Wizdle

[![Build, Test & Publish](https://github.com/lyndychivs/Wizdle/actions/workflows/build-test-publish.yaml/badge.svg?branch=main)](https://github.com/lyndychivs/Wizdle/actions/workflows/build-test-publish.yaml)
[![Solve Wordle Today](https://github.com/lyndychivs/Wizdle/actions/workflows/solve-wordle.yaml/badge.svg?branch=main)](https://github.com/lyndychivs/Wizdle/actions/workflows/solve-wordle.yaml)
[![Mutation testing badge](https://img.shields.io/endpoint?style=flat&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2Flyndychivs%2FWizdle%2Fmain)](https://dashboard.stryker-mutator.io/reports/github.com/lyndychivs/Wizdle/main)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Wizdle?style=flat&logo=nuget)](https://www.nuget.org/packages/Wizdle/)

A library for solving Wordle challenges in C#

| Prerequisite             | Note                                                                                                                                                                                                                          |
| :----------------------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `ILogger` implementation | This library requires an implementation of `Microsoft.Extensions.Logging` to construct. (See my `SeriLog` example [here](https://github.com/lyndychivs/Wizdle/blob/main/Wizdle.Integration.Tests/WizdleEngineTests.cs#L273).) |

## Example

```csharp
var wizdleEngine = new WizdleEngine(_logger);

var request = new WizdleRequest
{
    CorrectLetters = "....t"
    MisplacedLetters = "..rs.",
    ExcludeLetters = "haebu",
};

WizdleResponse response = wizdleEngine.ProcessWizdleRequest(request);
// response.Words: ["skirt", "snort", "sport"]
```

## Tests

All Unit Tests can be found under the [Wizdle.Unit.Tests](https://github.com/lyndychivs/Wizdle/tree/main/Wizdle.Unit.Tests) namespace.

## Package

Available on:

- [NuGet](https://www.nuget.org/packages/Wizdle/)
- [GitHub](https://github.com/lyndychivs/Wizdle/pkgs/nuget/Wizdle)
