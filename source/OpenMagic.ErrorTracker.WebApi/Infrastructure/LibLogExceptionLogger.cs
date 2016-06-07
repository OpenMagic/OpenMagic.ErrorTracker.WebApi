using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using OpenMagic.ErrorTracker.WebApi.Logging;

namespace OpenMagic.ErrorTracker.WebApi.Infrastructure
{
    public class LibLogExceptionLogger : IExceptionLogger
    {
        private static readonly ILog DefaultLogger = LogProvider.For<LibLogExceptionLogger>();
        private readonly ILog _log;

        public LibLogExceptionLogger()
            : this(DefaultLogger)
        {
        }

        internal LibLogExceptionLogger(ILog log)
        {
            _log = log;
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() => _log.ErrorException(context.Exception.Message, context.Exception), cancellationToken);
        }
    }
}