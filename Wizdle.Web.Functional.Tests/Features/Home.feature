Feature: Home
Users can successfully navigate the Wizdle Home page and interact with its elements.

Scenario: Users can navigate to the Wizdle Home page
    When I navigate to the Wizdle Home page
    Then the Page title should be "Wizdle | Solve Wordle..."
    And the Home page should display all expected elements

Scenario: Users can enable Dark Mode
    Given I navigate to the Wizdle Home page
    And the Home page theme is using Default colors
    When on the Home page, I click on the Dark Mode button
    Then the Home page theme is using DarkMode colors

Scenario Outline: User can find only one Word when specifing all Correct Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Correct Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        | <Char1>  | <Char2>  | <Char3>  | <Char4>  | <Char5>  |
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "PERCH"
    Examples:
    | Char1 | Char2 | Char3 | Char4 | Char5 |
    | P     | E     | R     | C     | H     |
    | p     | e     | r     | c     | h     |

Scenario Outline: User can find only one Word when specifing all Misplaced Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Misplaced Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        | <Char1>  | <Char2>  | <Char3>  | <Char4>  | <Char5>  |
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "PERCH"
    Examples:
    | Char1 | Char2 | Char3 | Char4 | Char5 |
    | E     | P     | H     | R     | C     |
    | e     | p     | h     | r     | c     |

Scenario Outline: User can find only one Word when specifing many Excluded Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Excluded Letters as "<Letters>"
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "ZONAL"
    Examples:
    | Letters               |
    | BCDEFGHIJKMPQRSTUVWXY |
    | bcdefghijkmpqrstuvwxy |

Scenario Outline: User can find no Words when specifing all Excluded Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Excluded Letters as "<Letters>"
    And on the Home page, I click on the Search button
    Then on the Home page, no Possible Words should be displayed
    Examples:
    | Letters                    |
    | ABCDEFGHIJKLMNOPQRSTUVWXYZ |
    | abcdefghijklmnopqrstuvwxyz |

Scenario: User can resolve the Correct Word by combining Correct, Misplaced and Excluded Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Correct Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        | A        | L        |          |          |          |
    And on the Home page, I specify the Misplaced Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        |          | E        |          |          | R        |
    And on the Home page, I specify the Excluded Letters as "BCDFGHIJKMNOPQSUVWXYZ"
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "ALERT"

Scenario: User can resolve the Correct Word after multiple searches
    Given I navigate to the Wizdle Home page
    And on the Home page, I specify the Excluded Letters as "BCDFGHIJKMNOPQSUVWXYZ"
    And on the Home page, I click on the Search button
    And on the Home page, the Possible Words should display multiple words
    And on the Home page, I specify the Correct Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        | A        | L        |          |          |          |
    And on the Home page, I click on the Search button
    And on the Home page, the Possible Words should display multiple words
    When on the Home page, I specify the Misplaced Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        |          | E        |          |          | R        |
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "ALERT"
