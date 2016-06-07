using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Anotar.LibLog;
using Mindscape.Raygun4Net.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;

namespace OpenMagic.ErrorTracker.WebApi.ModelBinders
{
    public class RaygunMessageBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(RaygunMessage))
            {
                return false;
            }

            try
            {
                if (bindingContext.Model != null)
                {
                    throw new ArgumentException("Expected Model to be null.", nameof(bindingContext));
                }

                var modelAsString = ReadModel(actionContext.Request);
                var modelAsJObject = Deserialize(modelAsString);

                ValidateJson(modelAsJObject);
                bindingContext.Model = Deserialize(modelAsString, modelAsJObject);
            }
            catch (ArgumentException exception)
            {
                LogTo.WarnException(exception.Message, exception);
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message);
            }
            catch (Exception exception)
            {
                LogTo.Warn($"Could not bind model.\n\n{exception}");
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, exception.Message);
            }

            // Even though we may not have been able bind the model we return true so no other model binder tries to bind this request to RaygunMessage.
            return true;
        }

        private static RaygunMessage Deserialize(string modelAsString, JToken modelAsJObject)
        {
            try
            {
                return modelAsJObject.ToObject<RaygunMessage>();
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not deserialize '{modelAsString}' as RaygunMessage.", exception);
            }
        }

        private static JObject Deserialize(string modelAsString)
        {
            try
            {
                return JsonConvert.DeserializeObject<JObject>(modelAsString);
            }
            catch (Exception exception)
            {
                throw new Exception($"Could not deserialize '{modelAsString}' as JObject.", exception);
            }
        }

        private static string ReadModel(HttpRequestMessage request)
        {
            try
            {
                return request.Content.ReadAsStringAsync().Result;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not read request content as string.", exception);
            }
        }

        private void ValidateJson(JObject jObject)
        {
            var expectedProperties = new[] {nameof(RaygunMessage.OccurredOn), nameof(RaygunMessage.Details)};

            if (jObject.HasProperties(expectedProperties))
            {
                return;
            }

            throw new Exception($"Expected content to include properties '{string.Join(", ", expectedProperties)}' but found properties '{string.Join(", ", jObject.Properties().Select(p => p.Name))}'.");
        }
    }
}