using System;
using System.Net.Http;
using System.Net.Http.Headers;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers;
using OpenMagic.ErrorTracker.WebApi.Specifications.Settings;
using TechTalk.SpecFlow;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.Features.Steps
{
    [Binding]
    public class WebApiServerSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;
        private readonly WebApiSettings _webApiSettings;

        // ReSharper disable once UnusedParameter.Local because all we actually need is the server to start and receiving it as a parameter achieves this
        public WebApiServerSteps(Given given, Actual actual, WebApiSettings webApiSettings, WebApiServer webApiServer)
        {
            _given = given;
            _actual = actual;
            _webApiSettings = webApiSettings;
        }

        [When(@"POST /errors is called")]
        public void WhenPOSTErrorsIsCalled()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_webApiSettings.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("X-ApiKey", _given.RaygunApiKey);

                _actual.Response = client.PostAsJsonAsync("errors", _given.PostBody).Result;
            }
        }
    }
}