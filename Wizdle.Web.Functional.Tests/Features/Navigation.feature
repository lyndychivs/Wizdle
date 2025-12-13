Feature: Navigation
Users can navigate the Wizdle Website.

Scenario: Users can navigate to the Wizdle Home page
    When I navigate to the Wizdle Home page
    Then the Page title should be "Wizdle | Solve Wordle..."
