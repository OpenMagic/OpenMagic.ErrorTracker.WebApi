Feature: OnAuthenticationAsync

Scenario: Known Api Key
    Given the request has a known api key
    When AuthenticateApiKeyFilterAttribute.OnAuthenticationAsync(HttpAuthenticationContext, CancellationToken) is called
    Then HttpAuthenticationContext.ErrorResult should be null

Scenario: Unknown Api Key
    Given the request has an unknown api key
    When AuthenticateApiKeyFilterAttribute.OnAuthenticationAsync(HttpAuthenticationContext, CancellationToken) is called
    Then HttpAuthenticationContext.ErrorResult should be set to StatusCodeResult Forbidden

Scenario: Api Key is not in header
    Given the request does not have an api key
    When AuthenticateApiKeyFilterAttribute.OnAuthenticationAsync(HttpAuthenticationContext, CancellationToken) is called
    Then HttpAuthenticationContext.ErrorResult should be set to StatusCodeResult Forbidden
