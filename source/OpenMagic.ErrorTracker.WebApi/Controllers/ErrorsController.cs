using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Mindscape.Raygun4Net.Messages;
using OpenMagic.ErrorTracker.Core.Events;
using OpenMagic.ErrorTracker.Core.Queues;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;

namespace OpenMagic.ErrorTracker.WebApi.Controllers
{
    public class ErrorsController : ApiController
    {
        private readonly IEventsQueue _eventsQueue;

        public ErrorsController(IEventsQueue eventsQueue)
        {
            _eventsQueue = eventsQueue;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync(RaygunMessage message)
        {
            await _eventsQueue.AddAsync(new RaygunMessageReceived(Request.GetApiKey(), message));

            return StatusCode(HttpStatusCode.Accepted);
        }
    }
}