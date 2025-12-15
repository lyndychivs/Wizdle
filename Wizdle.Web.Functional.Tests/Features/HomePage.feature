Feature: Home Page
Users can successfully navigate the Wizdle Home page and interact with its elements.

Scenario: Users can navigate to the Wizdle Home page
    When I navigate to the Wizdle Home page
    Then the Page title should be "Wizdle | Solve Wordle..."
    Then the Home page should display all expected elements

Scenario: Users can enable Dark Mode
    Given I navigate to the Wizdle Home page
    And the Home page theme is using Default colors
    When on the Home page, I click on the Dark Mode button
    Then the Home page theme is using DarkMode colors

Scenario Outline: User can find only one Word when specifing all Correct Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Correct Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        | <char1>  | <char2>  | <char3>  | <char4>  | <char5>  |
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "PERCH"
    Examples:
    | char1 | char2 | char3 | char4 | char5 |
    | P     | E     | R     | C     | H     |
    | p     | e     | r     | c     | h     |

Scenario Outline: User can find only one Word when specifing all Misplaced Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Misplaced Letters as
        | Letter 1 | Letter 2 | Letter 3 | Letter 4 | Letter 5 |
        | <char1>  | <char2>  | <char3>  | <char4>  | <char5>  |
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "PERCH"
    Examples:
    | char1 | char2 | char3 | char4 | char5 |
    | E     | P     | H     | R     | C     |
    | e     | p     | h     | r     | c     |

Scenario Outline: User can find only one Word when specifing many Excluded Letters
    Given I navigate to the Wizdle Home page
    When on the Home page, I specify the Excluded Letters as "<letters>"
    And on the Home page, I click on the Search button
    Then on the Home page, the Possible Words should display only one word, "ZONAL"
    Examples:
    | Letters               |
    | BCDEFGHIJKMPQRSTUVWXY |
    | bcdefghijkmpqrstuvwxy |
