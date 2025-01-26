Feature: EPAM Services Navigation

Scenario Outline: Navigate to specific service category
    Given I am on EPAM homepage
    When I click on Services in the main navigation menu
    And I select service category '<category>'
    And I click on the '<buttonName>' button on the '<category>' page
    Then I should see the correct title for '<buttonName>'

Examples:
    | category              | buttonName                    |
    | Artificial Intelligence | Generative AI         |