using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Anotar.LibLog;
using Microsoft.Owin;
using Mindscape.Raygun4Net.Messages;
using Mindscape.Raygun4Net.WebApi;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using OpenMagic.AspNet.WebApi.Filters;
using OpenMagic.ErrorTracker.WebApi;
using OpenMagic.ErrorTracker.WebApi.Filters;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.ModelBinders;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace OpenMagic.ErrorTracker.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // todo: Uncomment when IoC.CreateKernel() has been fully implemented
            // Configuration(appBuilder, IoC.CreateKernel());
            throw new NotImplementedException();
        }

        public void Configuration(IAppBuilder appBuilder, IKernel kernel)
        {
            var name = typeof(Startup).Namespace;

            try
            {
                LogTo.Debug($"Configuring {name}...");

                var config = new HttpConfiguration();

                RaygunWebApiClient.Attach(config);

                config.BindParameter(typeof(RaygunMessage), new RaygunMessageBinder());
                config.Filters.Add(new ModelStateFilterAttribute());
                config.Filters.Add(kernel.Get<AuthenticateApiKeyFilterAttribute>());
                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new {id = RouteParameter.Optional});
                config.Services.Add(typeof(IExceptionLogger), new LibLogExceptionLogger());

                appBuilder.UseNinjectMiddleware(() => kernel).UseNinjectWebApi(config);

                LogTo.Debug($"Successfully configured {name}.");
            }
            catch (Exception exception)
            {
                LogTo.ErrorException($"Configuring {name} failed.", exception);
                throw;
            }
        }
    }
}