# 💻 Wizdle.Console
Specify the following commands & arguments:

## 🛠️Command Line Interface Usages
### 🧩 solve
Attempts to guess the Word by filtering from the provided arguments.
```bash
solve [parameters]
```
#### Parameters
| Argument                | Description | Default | Required |
| :---                    | :---        | :---    | :---     |
| `--correct <letters>`   | Correct Letters known to exist in the Word.</br>Follow the format of `"a.b.c"` where unknown letters are represented by a dot (`.`). | — | No |
| `--misplaced <letters>` | Misplaced Letters known to exist in the Word.</br>Follow the format of `"a.b.c"` where unknown letters are represented by a dot (`.`). | — | No |
| `--exclude <letters>`   | Letters that are known to not exist in the Word.</br>Follow the format of `"abc"` where each letter is a single character. | — | No |

#### Example
**Command:**
```bash
$ ./Wizdle.Console.exe solve --correct "....t" --misplaced "..rs." --exclude "haebu"
```
**Response:**
```bash
Processing WizdleRequest: CorrectLetters: "....t"   MisplacedLetters: "..rs." ExcludeLetters: "haebu"
Mapping WizdleRequest:    CorrectLetters: "....t"   MisplacedLetters: "..rs." ExcludeLetters: "haebu"
Mapped SolveParameters:   CorrectLetters: "????t"   MisplacedLetters: "??rs?" ExcludeLetters: "haebu"
Found 3          Word(s) matching the criteria.
Found 3 Word(s) matching the criteria.
skirt
snort
sport
```
