# Wizdle.Domain
Class Domain model created using [PlantUML](https://plantuml.com/).

```mermaid
!theme plain

~interface IWords {
    +IEnumerable<string> GetWords()
}

~class Words {
    +IEnumerable<string> GetWords()
}

IWords <|-- Words

~interface IRequestMapper {
    +SolveParameters MapToSolveParameters(WizdleRequest)
}

~class RequestMapper {
    -ILogger _logger
    +SolveParameters MapToSolveParameters(WizdleRequest)
}

IRequestMapper <|-- RequestMapper

~interface IWordRepository {
    +IEnumerable<string> GetWords()
}

~class WordRepository {
    -ILogger _logger
    -IWords _words
    +IEnumerable<string> GetWords()
}

IWordRepository <|-- WordRepository
WordRepository --> IWords

~interface IWordSolver {
    +IEnumerable<string> Solve(SolveParameters)
}

~class WordSolver {
    -ILogger _logger
    -IWordRepository _wordRepository
    -ISolveParametersValidator _wordParameterValidator
    -IEnumerable<string> _words
    +IEnumerable<string> Solve(SolveParameters)
}

~class SolveParameters {
    +IList<char> CorrectLetters
    +IList<char> MisplacedLetters
    +IList<char> ExcludeLetters
}

IWordSolver <|-- WordSolver
IWordSolver --> SolveParameters : uses
WordSolver --> SolveParameters : uses
WordSolver --> IWordRepository
WordSolver --> ISolveParametersValidator

~interface ISolveParametersValidator {
    +bool IsValid(SolveParameters)
}

~class SolveParametersValidator {
    -ILogger _logger
    +bool IsValid(SolveParameters)
}

ISolveParametersValidator <|-- SolveParametersValidator

~interface IRequestValidator {
    +IEnumerable<string> GetErrors(WizdleRequest)
}

~class RequestValidator {
    -ILogger _logger
    +IEnumerable<string> GetErrors(WizdleRequest)
}

IRequestValidator <|-- RequestValidator

+class WizdleEngine {
    -ILogger _logger
    -IRequestValidator _requestValidator
    -IWordSolver _wordSolver
    -IRequestMapper _requestMapper
    +WizdleResponse ProcessWizdleRequest(WizdleRequest)
}

WizdleEngine --> IRequestValidator
WizdleEngine --> IWordSolver
WizdleEngine --> IRequestMapper

+class WizdleResponse {
    +IEnumerable<string> Messages
    +IEnumerable<string> Words
}

+class WizdleRequest {
    +string CorrectLetters
    +string MisplacedLetters
    +string ExcludeLetters
}

WizdleEngine --> WizdleRequest : uses
WizdleEngine --> WizdleResponse : uses

+interface ILogger {
}

WizdleEngine --> ILogger

```
