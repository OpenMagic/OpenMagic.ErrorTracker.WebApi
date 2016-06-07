using System;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using FakeItEasy;
using Mindscape.Raygun4Net.Messages;
using Newtonsoft.Json.Linq;
using OpenMagic.ErrorTracker.Core.Queues;
using OpenMagic.ErrorTracker.Core.Repositories;
using OpenMagic.ErrorTracker.WebApi.Filters;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.Logging;
using OpenMagic.ErrorTracker.WebApi.ModelBinders;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers.Fakes;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Helpers
{
    public class Given
    {
        public Given()
        {
            var httpRequestContext = new HttpRequestContext();
            var httpRequestMessage = new HttpRequestMessage();
            var httpControllerDescriptor = new HttpControllerDescriptor();
            var httpController = A.Fake<IHttpController>();
            var principal = A.Fake<IPrincipal>();

            AuthenticateApiKeyRepository = new InMemoryAuthenticateApiKeyRepository(new[] {IoC.FakeRaygunApiKey});
            CancellationToken = new CancellationToken();
            EventsQueue = new InMemoryEventsQueue();
            HttpControllerContext = new HttpControllerContext(httpRequestContext, httpRequestMessage, httpControllerDescriptor, httpController);
            HttpActionDescriptor = new ReflectedHttpActionDescriptor();
            HttpActionContext = new HttpActionContext(HttpControllerContext, HttpActionDescriptor);
            HttpAuthenticationContext = new HttpAuthenticationContext(HttpActionContext, principal);
            ModelBindingContext = new ModelBindingContext();
        }

        public CancellationToken CancellationToken { get; set; }
        public HttpAuthenticationContext HttpAuthenticationContext { get; set; }
        public string RaygunApiKey { get; set; }
        public RaygunMessage RaygunMessage { get; set; }
        public object PostBody { get; set; }
        internal ILog Logger { get; set; }
        public Exception Exception { get; set; }
        public ModelBindingContext ModelBindingContext { get; set; }
        public HttpActionContext HttpActionContext { get; set; }
        public HttpControllerContext HttpControllerContext { get; set; }
        public HttpActionDescriptor HttpActionDescriptor { get; set; }
        public RaygunMessageBinder RaygunMessageBinder { get; set; }
        public JObject JObject { get; set; }
        public AuthenticateApiKeyFilterAttribute AuthenticateApiKeyFilterAttribute { get; set; }
        public IAuthenticateApiKeyRepository AuthenticateApiKeyRepository { get; set; }
        public IEventsQueue EventsQueue { get; set; }

        public InMemoryEventsQueue InMemoryEventsQueue
        {
            get
            {
                var queue = EventsQueue as InMemoryEventsQueue;

                if (queue != null)
                {
                    return queue;
                }

                throw new Exception($"Current event queue is {EventsQueue.GetType()} not {nameof(InMemoryEventsQueue)}.");
            }
        }
    }
}