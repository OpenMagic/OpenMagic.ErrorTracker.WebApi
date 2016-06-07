using Microsoft.Owin.Hosting;
using OpenMagic.ErrorTracker.Core.Queues;
using OpenMagic.ErrorTracker.Core.Repositories;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.Specifications.Settings;
using Owin;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Helpers
{
    public class WebApiServer
    {
        private readonly Given _given;

        public WebApiServer(WebApiSettings webApiSettings, Given given)
        {
            _given = given;

            WebApp.Start(webApiSettings.BaseUri, Startup);
        }

        private void Startup(IAppBuilder appBuilder)
        {
            var startup = new Startup();
            var kernel = IoC.CreateKernel();

            kernel.Rebind<IAuthenticateApiKeyRepository>().ToConstant(_given.AuthenticateApiKeyRepository);
            kernel.Rebind<IEventsQueue>().ToConstant(_given.EventsQueue);

            startup.Configuration(appBuilder, kernel);
        }
    }
}