using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace OpenMagic.ErrorTracker.WebApi.Infrastructure
{
    public static class JObjectExtensions
    {
        public static bool HasProperties(this JObject value, params string[] propertyNames)
        {
            return propertyNames.All(propertyName => value.Properties().Any(jProperty => jProperty.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}