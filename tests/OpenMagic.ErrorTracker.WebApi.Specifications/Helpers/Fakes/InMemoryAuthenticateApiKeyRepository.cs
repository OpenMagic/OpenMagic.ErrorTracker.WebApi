using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenMagic.ErrorTracker.Core.Repositories;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Helpers.Fakes
{
    public class InMemoryAuthenticateApiKeyRepository : IAuthenticateApiKeyRepository
    {
        private readonly string[] _apiKeys;

        public InMemoryAuthenticateApiKeyRepository(IEnumerable<string> apiKeys)
        {
            _apiKeys = apiKeys.ToArray();
        }

        public Task<bool> AuthenticateAsync(string apiKey)
        {
            return Task.FromResult(_apiKeys.Contains(apiKey));
        }
    }
}