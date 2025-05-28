# Wizdle
A library for solving Wordle challenges in C#

| Prerequisite        | Note |
| :---                | :--- |
| .NET8 SDK           | .NET8 or greater required.<br/>Check current .NET version `dotnet --version`.<br/>Download .NET8 [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). |
| `ILogger` implementation|This library requires an implementation of `Microsoft.Extensions.Logging` to construct.<br/>(See my `SeriLog` example [here](https://github.com/lyndychivs/Wizdle/blob/master/Wizdle.IntegrationTests/WizdleEngineTests.cs#L122).)|

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
All Unit Tests can be found under the [Wizdle.Tests](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.Tests) namespace.

## Package
Available on:
- [NuGet](https://www.nuget.org/packages/Wizdle/)
- [GitHub](https://github.com/lyndychivs/Wizdle/pkgs/nuget/Wizdle)
