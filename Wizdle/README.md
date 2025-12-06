# Wizdle
A library for solving Wordle challenges in C#

| Prerequisite        | Note |
| :---                | :--- |
| `ILogger` implementation|This library requires an implementation of `Microsoft.Extensions.Logging` to construct. (See my `SeriLog` example [here](https://github.com/lyndychivs/Wizdle/blob/master/Wizdle.Integration.Tests/WizdleEngineTests.cs#L273).)|

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
All Unit Tests can be found under the [Wizdle.Unit.Tests](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.Unit.Tests) namespace.

## Package
Available on:
- [NuGet](https://www.nuget.org/packages/Wizdle/)
- [GitHub](https://github.com/lyndychivs/Wizdle/pkgs/nuget/Wizdle)
