Feature: HasProperties

Scenario: All properties exist
	Given JObject has properties 'A, B, C'
	When I call JObjectExtensions.HasProperties(jObject, 'A, B, C')
	Then the result should be true

Scenario: Some properties exist
	Given JObject has properties 'A, B, C'
	When I call JObjectExtensions.HasProperties(jObject, 'A, B, D')
	Then the result should be false

Scenario: No properties exist
	Given JObject has properties 'A, B, C'
	When I call JObjectExtensions.HasProperties(jObject, 'E, F, G')
	Then the result should be false

Scenario: All properties and others exist
	Given JObject has properties 'A, B, C'
	When I call JObjectExtensions.HasProperties(jObject, 'A, B')
	Then the result should be true
