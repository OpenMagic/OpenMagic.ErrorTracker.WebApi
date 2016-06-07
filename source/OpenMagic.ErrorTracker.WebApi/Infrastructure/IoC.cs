using Ninject;

namespace OpenMagic.ErrorTracker.WebApi.Infrastructure
{
    public class IoC
    {
        public const string FakeRaygunApiKey = "todo-remove-this-dummy-api-key-";

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            // todo

            return kernel;
        }
    }
}