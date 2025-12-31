@a11y
Feature: Accessibility
Validate that Wizdle Web pages meet accessibility standards.

Scenario: User navigates to the Wizdle Home page and performs Accessibility Testing
    Given I navigate to the Wizdle Home page
    And on the Home page, I click on the Search button
    And on the Home page, the Possible Words should display multiple words
    When I perform an Accessibility audit on the current Page
    Then the Page should have no Accessibility Violations

Scenario: User navigates to the Wizdle Home page and performs Accessibility Testing in Dark Mode
    Given I navigate to the Wizdle Home page
    And on the Home page, I click on the Dark Mode button
    And on the Home page, I click on the Search button
    And on the Home page, the Possible Words should display multiple words
    When I perform an Accessibility audit on the current Page
    Then the Page should have no Accessibility Violations
