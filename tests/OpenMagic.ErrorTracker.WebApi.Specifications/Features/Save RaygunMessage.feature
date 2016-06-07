Feature: Save RaygunMessage
	I want to save a RaygunMessage posted to /errors

Scenario: Post RaygunMessage with known ApiKey
    Given POST headers contains a known X-ApiKey
    And POST body contains a valid RaygunMessage
    When POST /errors is called
    Then RaygunMessageReceived event is added events queue
    Then the response status code should be 'Accepted'
    And the response reason phrase should be 'Accepted'
    And the response content should be empty

Scenario: Post RaygunMessage with unknown ApiKey
    Given POST headers contains an unknown X-ApiKey
    And POST body contains a valid RaygunMessage
    When POST /errors is called
    Then the response status code should be 'Forbidden'
    And the response reason phrase should be 'Forbidden'
    And the response content should be empty

Scenario: Post with random message
    Given POST headers contains a known X-ApiKey
    And POST body contains a random message
    When POST /errors is called
    Then the response status code should be 'BadRequest'
    And the response reason phrase should be 'Bad Request'
    And the response content should be error message 'Expected content to include properties 'OccurredOn, Details' but found properties 'dummy'.'
