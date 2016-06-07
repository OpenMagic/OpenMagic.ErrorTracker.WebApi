using System.Web.Http.Dependencies;
using NullGuard;

namespace OpenMagic.ErrorTracker.WebApi.Infrastructure
{
    // ReSharper disable once InconsistentNaming
    public static class IDependencyResolverExtensions
    {
        [return: AllowNull]
        public static T GetService<T>(this IDependencyResolver dependencyResolver)
        {
            var service = dependencyResolver.GetService(typeof(T));

            return service == null ? default(T) : (T)service;
        }
    }
}