using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Metadata.Providers;
using FluentAssertions;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.UnitTests.Steps.ModelBinders.RaygunMessageBinder
{
    [Binding]
    public class BindModelSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public BindModelSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;

            _given.ModelBindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(null, typeof(RaygunMessage));
            _given.RaygunMessageBinder = new WebApi.ModelBinders.RaygunMessageBinder();
        }

        [Given(@"bindingContext\.ModelType is not a RaygunMessage")]
        public void GivenBindingContext_ModelTypeIsNotARaygunMessage()
        {
            _given.ModelBindingContext.ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(null, typeof(object));
        }

        [Given(@"bindingContext\.Model is not null")]
        public void GivenBindingContext_ModelIsNotNull()
        {
            _given.ModelBindingContext.Model = "dummy";
        }

        [Given(@"actionContext\.Request\.Content is a valid RaygunMessage")]
        public void GivenActionContext_Request_ContentIsAValidRaygunMessage()
        {
            _given.RaygunMessage = RaygunMessageBuilder.New
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

            GivenActionContext_Request_ContentIs(_given.RaygunMessage);
        }

        private void GivenActionContext_Request_ContentIs<T>(T content)
        {
            _given.HttpControllerContext.Request = new HttpRequestMessage
            {
                Content = new ObjectContent<T>(content, new JsonMediaTypeFormatter())
            };
        }

        [Given(@"actionContext\.Request\.Content is a invalid RaygunMessage")]
        public void GivenActionContext_Request_ContentIsAInvalidRaygunMessage()
        {
            GivenActionContext_Request_ContentIs(new {Dummy = "value"});
        }

        [When(@"RaygunMessageBinder\.BindModel\(HttpActionContext, ModelBindingContext\)")]
        public void WhenRaygunMessageBinder_BindModelHttpActionContextModelBindingContext()
        {
            _actual.Result = _given.RaygunMessageBinder.BindModel(_given.HttpActionContext, _given.ModelBindingContext);
        }

        [Then(@"actionContext\.Response should be HttpStatusCode\.InternalServerError")]
        public void ThenActionContext_ResponseShouldBeHttpStatusCode_InternalServerError()
        {
            ThenActionContext_Response_HttpStatusCode_ShouldBe(HttpStatusCode.InternalServerError);
        }

        private void ThenActionContext_Response_HttpStatusCode_ShouldBe(HttpStatusCode httpStatusCode)
        {
            _given.HttpActionContext.Response.StatusCode.As<HttpStatusCode>().Should().Be(httpStatusCode);
        }

        [Then(@"bindingContext\.Model should be actionContext\.Request\.Content as a RaygunMessage")]
        public void ThenBindingContext_ModelShouldBeActionContext_Request_ContentAsARaygunMessage()
        {
            _given.ModelBindingContext.Model.ShouldBeEquivalentTo(_given.RaygunMessage);
        }

        [Then(@"actionContext\.Response should be HttpStatusCode\.BadRequest")]
        public void ThenActionContext_ResponseShouldBeHttpStatusCode_BadRequest()
        {
            ThenActionContext_Response_HttpStatusCode_ShouldBe(HttpStatusCode.BadRequest);
        }

        [Then(@"actionContext\.Response should be null")]
        public void ThenActionContext_ResponseShouldBeNull()
        {
            _given.HttpActionContext.Response.Should().BeNull();
        }
    }
}