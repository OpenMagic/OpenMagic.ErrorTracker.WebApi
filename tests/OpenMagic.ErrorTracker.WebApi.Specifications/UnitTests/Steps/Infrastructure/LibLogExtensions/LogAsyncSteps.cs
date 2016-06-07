using System;
using System.Threading;
using System.Web.Http.ExceptionHandling;
using FakeItEasy;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.Logging;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.UnitTests.Steps.Infrastructure.LibLogExtensions
{
    [Binding]
    public class LogAsyncSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public LogAsyncSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"a logger")]
        public void GivenALogger()
        {
            _given.Logger = A.Fake<ILog>();

            A.CallTo(() => _given.Logger.Log(A<LogLevel>.Ignored, null, A<Exception>.Ignored, A<object[]>.Ignored))
                .Returns(true);
        }

        [Given(@"an exception")]
        public void GivenAnException()
        {
            _given.Exception = new Exception("dummy exception message");
        }

        [When(@"new LibLogExceptionLogger\(logger\) is called")]
        public void WhenNewLibLogExceptionLoggerLoggerIsCalled()
        {
            _actual.LibLogExceptionLogger = new LibLogExceptionLogger(_given.Logger);
        }

        [When(@"LibLogExceptionLogger\.LogAsync\(ExceptionLoggerContext, CancellationToken\) is called")]
        public void WhenLibLogExceptionLogger_LogAsyncExceptionLoggerContextCancellationTokenIsCalled()
        {
            var exceptionContextCatchBlock = new ExceptionContextCatchBlock("dummy", false, false);
            var exceptionContext = new ExceptionContext(_given.Exception, exceptionContextCatchBlock);
            var exceptionLoggerContext = new ExceptionLoggerContext(exceptionContext);
            var cancellationToken = new CancellationToken();

            _actual.LibLogExceptionLogger.LogAsync(exceptionLoggerContext, cancellationToken).Wait(cancellationToken);
        }

        [Then(@"logger\.ErrorException\(string, Exception\) should be called")]
        public void ThenLogger_ErrorExceptionStringExceptionShouldBeCalled()
        {
            A.CallTo(_given.Logger)
                .Where(call => call.Method.Name == nameof(ILog.Log))
                .WhenArgumentsMatch(arguments =>
                {
                    var logLevel = (LogLevel)arguments[0];
                    var messageFunc = arguments[1] as Func<string>;
                    var exception = (Exception)arguments[2];

                    return logLevel == LogLevel.Error
                           && messageFunc != null
                           && messageFunc().Equals(_given.Exception.Message)
                           && exception != null
                           && exception.Equals(_given.Exception);
                })
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}