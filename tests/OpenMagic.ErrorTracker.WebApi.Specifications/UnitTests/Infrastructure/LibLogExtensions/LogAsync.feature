Feature: LogAsync

Scenario: Log exception
    Given a logger
    And an exception
    When new LibLogExceptionLogger(logger) is called
	And LibLogExceptionLogger.LogAsync(ExceptionLoggerContext, CancellationToken) is called
	Then logger.ErrorException(string, Exception) should be called
