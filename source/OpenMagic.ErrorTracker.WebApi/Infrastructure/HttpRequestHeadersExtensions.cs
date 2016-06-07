using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using OpenMagic.ErrorTracker.WebApi.Exceptions;

namespace OpenMagic.ErrorTracker.WebApi.Infrastructure
{
    // todo: move to NuGet package
    public static class HttpRequestHeadersExtensions
    {
        public static IEnumerable<string> FindValues(this HttpRequestHeaders headers, string name)
        {
            IEnumerable<string> apiKeys;
            headers.TryGetValues(name, out apiKeys);
            return apiKeys ?? Enumerable.Empty<string>();
        }

        public static string GetValue(this HttpRequestHeaders headers, string name)
        {
            var values = headers.FindValues(name).ToArray();

            if (values.Length == 0)
            {
                throw new RequestHeaderException($"Cannot find request header '{name}'.");
            }

            if (values.Length > 1)
            {
                throw new RequestHeaderException($"Found request header '{name}' with values '{string.Join(",", values)}' but only one '{name}' value is allowed.");
            }

            return values[0];
        }
    }
}