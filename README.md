<p align="center"><img src="Resources/WizdleTitle.png" alt="Test Miner" width="512" height="512"></p>

[![Mutation testing badge](https://img.shields.io/endpoint?style=for-the-badge&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2Flyndychivs%2FWizdle%2Fmaster)](https://dashboard.stryker-mutator.io/reports/github.com/lyndychivs/Wizdle/master)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Wizdle?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/Wizdle/)

# 🔭 High Level
A library for solving Wordle.

**Simple Flow:**

```mermaid
---
config:
  theme: redux-dark
  look: handDrawn
  layout: fixed
---
flowchart LR
    CORRECT["Correct Letters"] --> REQUEST["Request"]
    MISPLACED["Misplaced Letters"] --> REQUEST
    EXCLUDE["Excluded Letters"] --> REQUEST
    REQUEST --> ENGINE["Engine"]
    ENGINE --> RESPONSE["Response"]
    RESPONSE --> WORDS["Possible Words"]
```

## Prerequisites
| Prerequisite        | Note |
| :---                | :--- |
| .NET8 SDK           | .NET8 or greater required.<br/>Check current .NET version `dotnet --version`.<br/>Download .NET8 [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). |

# 🧙 Wizdle
The Wizdle core library found [here](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle), is responsbile for translating the request into a list of possible Words.

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

## Package
Available on:
- [NuGet](https://www.nuget.org/packages/Wizdle/)
- [GitHub](https://github.com/lyndychivs/Wizdle/pkgs/nuget/Wizdle)

# 💻 Wizdle.Console
The Wizdle Console application allows us to access all the functionality of the Wizdle library via the CLI.

More information can be found [here](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.Console)

## Example
**Command:**
```
$ ./Wizdle.Console.exe solve --correct "....t" --misplaced "..rs." --exclude "haebu"
```
**Response:**
```
Processing WizdleRequest: CorrectLetters: "....t"   MisplacedLetters: "..rs." ExcludeLetters: "haebu"
Mapping WizdleRequest:    CorrectLetters: "....t"   MisplacedLetters: "..rs." ExcludeLetters: "haebu"
Mapped SolveParameters:   CorrectLetters: "????t"   MisplacedLetters: "??rs?" ExcludeLetters: "haebu"
Found 3          Word(s) matching the criteria.
Found 3 Word(s) matching the criteria.
skirt
snort
sport
```

# 🪟 Wizdle.Windows
The Wizdle Windows application allows us to access all the functionality of the Wizdle library via a GUI.

More information can be found [here](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.Windows).

![Wizdle.Windows](Wizdle.Windows/Resources/Wizdle.Windows.png)

# 🧪 Testing
- Unit Testing
  - [Wizdle.Tests](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.Tests)
- Integration Testing
  - [Wizdle.IntegrationTests](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.IntegrationTests)
- Performance Testing
  - [Wizdle.PerformanceTests](https://github.com/lyndychivs/Wizdle/tree/master/Wizdle.PerformanceTests)
- Mutation Testing
  - [Strkyer.NET](https://dashboard.stryker-mutator.io/reports/github.com/lyndychivs/Wizdle/master) with [my GitHub Action](https://github.com/lyndychivs/dotnet-stryker-action)
