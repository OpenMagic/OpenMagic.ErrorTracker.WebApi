using System.Net.Http;

namespace OpenMagic.ErrorTracker.WebApi.Infrastructure
{
    public static class HttpRequestMessageExtensions
    {
        public const string ApiKeyHeaderName = "X-ApiKey";

        // todo: unit test
        public static string GetApiKey(this HttpRequestMessage request)
        {
            return request.Headers.GetValue(ApiKeyHeaderName);
        }
    }
}