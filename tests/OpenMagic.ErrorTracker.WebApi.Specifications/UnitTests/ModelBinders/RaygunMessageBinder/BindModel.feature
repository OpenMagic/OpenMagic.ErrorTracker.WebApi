Feature: BindModel

Scenario: model type is not RaygunMessage
    Given bindingContext.ModelType is not a RaygunMessage
    When RaygunMessageBinder.BindModel(HttpActionContext, ModelBindingContext)
    Then the result should be false

Scenario: model is not null
    Given bindingContext.Model is not null
    When RaygunMessageBinder.BindModel(HttpActionContext, ModelBindingContext)
    Then the result should be true
    And actionContext.Response should be HttpStatusCode.InternalServerError

Scenario: request content is a valid RaygunMessage
    Given actionContext.Request.Content is a valid RaygunMessage
    When RaygunMessageBinder.BindModel(HttpActionContext, ModelBindingContext)
    Then the result should be true
    And actionContext.Response should be null
    And bindingContext.Model should be actionContext.Request.Content as a RaygunMessage

Scenario: request content is a invalid RaygunMessage
    Given actionContext.Request.Content is a invalid RaygunMessage
    When RaygunMessageBinder.BindModel(HttpActionContext, ModelBindingContext)
    Then the result should be true
    And actionContext.Response should be HttpStatusCode.BadRequest
