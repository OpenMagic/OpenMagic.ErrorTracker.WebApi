namespace OpenMagic.ErrorTracker.WebApi.Specifications.Settings
{
    public class WebApiSettings
    {
        private static int _lastPort = 9000;
        private readonly int _port;

        public WebApiSettings()
        {
            _port = ++_lastPort;
        }

        public string BaseUri => $"http://localhost:{_port}/";
    }
}