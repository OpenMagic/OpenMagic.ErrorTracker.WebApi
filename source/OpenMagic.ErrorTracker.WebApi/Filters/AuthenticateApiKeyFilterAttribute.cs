using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Anotar.LibLog;
using OpenMagic.ErrorTracker.Core.Repositories;
using OpenMagic.ErrorTracker.WebApi.Exceptions;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using WebApi.AuthenticationFilter;

namespace OpenMagic.ErrorTracker.WebApi.Filters
{
    public class AuthenticateApiKeyFilterAttribute : AuthenticationFilterAttribute
    {
        private readonly IAuthenticateApiKeyRepository _repository;

        public AuthenticateApiKeyFilterAttribute(IAuthenticateApiKeyRepository repository)
        {
            _repository = repository;
        }

        public override async Task OnAuthenticationAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (!await AuthenticateAsync(context))
            {
                context.ErrorResult = new StatusCodeResult(HttpStatusCode.Forbidden, context.Request);
            }
        }

        private async Task<bool> AuthenticateAsync(HttpAuthenticationContext context)
        {
            try
            {
                var apiKey = context.Request.GetApiKey();

                var authenticated = await _repository.AuthenticateAsync(apiKey);

                if (!authenticated)
                {
                    LogTo.Warn($"Denied access to ApiKey '{apiKey}'.");
                }

                return authenticated;
            }
            catch (RequestHeaderException exception)
            {
                LogTo.Warn(exception.Message);
                return false;
            }
        }
    }
}