using System;

namespace OpenMagic.ErrorTracker.WebApi.Exceptions
{
    public class RequestHeaderException : Exception
    {
        public RequestHeaderException(string message)
            : base(message)
        {
        }
    }
}