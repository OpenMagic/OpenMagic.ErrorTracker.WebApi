using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;
using Newtonsoft.Json;
using OpenMagic.ErrorTracker.Core.Events;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Features.Steps
{
    [Binding]
    public class SaveRaygunMessageSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public SaveRaygunMessageSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"POST headers contains a known X-ApiKey")]
        public void GivenPOSTHeadersContainsAKnownX_ApiKey()
        {
            _given.RaygunApiKey = IoC.FakeRaygunApiKey;
        }

        [Given(@"POST body contains a valid RaygunMessage")]
        public void GivenPOSTBodyContainsAValidRaygunMessage()
        {
            var raygunMessage = RaygunMessageBuilder.New
                .SetHttpDetails(null)
                .SetTimeStamp(DateTime.UtcNow)
                .SetEnvironmentDetails()
                .SetMachineName(Environment.MachineName)
                .SetExceptionDetails(new Exception("dummy exception"))
                .SetClientDetails()
                .SetVersion("1.0.0.0")
                .SetTags(new List<string>(new[] {"dummy", "tags"}))
                .SetUserCustomData(null)
                .SetUser(null)
                .Build();

            _given.PostBody = raygunMessage;
        }

        [Given(@"POST headers contains an unknown X-ApiKey")]
        public void GivenPOSTHeadersContainsAnUnknownX_ApiKey()
        {
            _given.RaygunApiKey = "dummy api key";
        }

        [Given(@"POST body contains a random message")]
        public void GivenPOSTBodyContainsARandomMessage()
        {
            _given.PostBody = new {dummy = "message"};
        }

        [Then(@"RaygunMessageReceived event is added events queue")]
        public void ThenRaygunMessageReceivedEventIsAddedEventsQueue()
        {
            var expected = new RaygunMessageReceived(_given.RaygunApiKey, _given.PostBody as RaygunMessage);
            var actual = _given.InMemoryEventsQueue.Events.Single() as RaygunMessageReceived;

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"the response status code should be '(.*)'")]
        public void ThenTheResponseStatusCodeShouldBe(string expectedStatusCode)
        {
            _actual.Response.StatusCode.ToString().Should().Be(expectedStatusCode);
        }

        [Then(@"the response reason phrase should be '(.*)'")]
        public void ThenTheReasonPhraseShouldBe(string expectedReasonPhrase)
        {
            _actual.Response.ReasonPhrase.Should().Be(expectedReasonPhrase);
        }

        [Then(@"the response content should be empty")]
        public void ThenTheResponseContentShouldBeEmpty()
        {
            _actual.Response.Content.ReadAsStringAsync().Result.Should().BeNullOrWhiteSpace();
        }

        [Then(@"the response content should be error message '(.*)'")]
        public void ThenTheResponseContentShouldBeErrorMessage(string expectedErrorMessage)
        {
            var expectedJson = JsonConvert.SerializeObject(new {Message = expectedErrorMessage});

            _actual.Response.Content.ReadAsStringAsync().Result.Should().Be(expectedJson);
        }
    }
}