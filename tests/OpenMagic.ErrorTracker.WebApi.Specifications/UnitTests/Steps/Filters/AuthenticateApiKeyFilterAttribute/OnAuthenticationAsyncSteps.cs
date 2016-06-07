using System.Net;
using FluentAssertions;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.UnitTests.Steps.Filters.AuthenticateApiKeyFilterAttribute
{
    [Binding]
    public class OnAuthenticationAsyncSteps
    {
        private readonly Given _given;

        public OnAuthenticationAsyncSteps(Given given)
        {
            _given = given;
        }

        [Given(@"the request has a known api key")]
        public void GivenTheRequestHasAKnownApiKey()
        {
            _given.HttpAuthenticationContext.Request.Headers.Add("X-ApiKey", IoC.FakeRaygunApiKey);
        }

        [Given(@"the request has an unknown api key")]
        public void GivenTheRequestHasAnUnknownApiKey()
        {
            _given.HttpAuthenticationContext.Request.Headers.Add("X-ApiKey", "dummy api key");
        }

        [Given(@"the request does not have an api key")]
        public void GivenTheRequestDoesNotHaveAnApiKey()
        {
            _given.HttpAuthenticationContext.Request.Headers.Remove("X-ApiKey");
        }

        [When(@"AuthenticateApiKeyFilterAttribute\.OnAuthenticationAsync\(HttpAuthenticationContext, CancellationToken\) is called")]
        public void WhenAuthenticateApiKeyFilterAttribute_OnAuthenticationAsyncHttpAuthenticationContextCancellationTokenIsCalled()
        {
            _given.AuthenticateApiKeyFilterAttribute = new WebApi.Filters.AuthenticateApiKeyFilterAttribute(_given.AuthenticateApiKeyRepository);
            _given.AuthenticateApiKeyFilterAttribute.OnAuthenticationAsync(_given.HttpAuthenticationContext, _given.CancellationToken).Wait();
        }

        [Then(@"HttpAuthenticationContext\.ErrorResult should be null")]
        public void ThenHttpAuthenticationContext_ErrorResultShouldBeNull()
        {
            _given.HttpAuthenticationContext.ErrorResult.Should().BeNull();
        }

        [Then(@"HttpAuthenticationContext\.ErrorResult should be set to StatusCodeResult Forbidden")]
        public void ThenHttpAuthenticationContext_ErrorResultShouldBeSetToStatusCodeResultForbidden()
        {
            _given.HttpAuthenticationContext.ErrorResult.ExecuteAsync(_given.CancellationToken).Result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}