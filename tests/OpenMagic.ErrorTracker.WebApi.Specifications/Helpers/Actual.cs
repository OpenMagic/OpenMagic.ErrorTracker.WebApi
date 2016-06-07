using System.Net.Http;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Helpers
{
    public class Actual
    {
        public HttpResponseMessage Response { get; set; }
        public LibLogExceptionLogger LibLogExceptionLogger { get; set; }
        public object Result { get; set; }
    }
}