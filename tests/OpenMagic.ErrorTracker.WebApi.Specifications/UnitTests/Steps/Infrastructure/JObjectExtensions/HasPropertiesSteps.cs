using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using OpenMagic.ErrorTracker.WebApi.Infrastructure;
using OpenMagic.ErrorTracker.WebApi.Specifications.Helpers;
using TechTalk.SpecFlow;

namespace OpenMagic.ErrorTracker.WebApi.Specifications.UnitTests.Steps.Infrastructure.JObjectExtensions
{
    [Binding]
    public class HasPropertiesSteps
    {
        private readonly Actual _actual;
        private readonly Given _given;

        public HasPropertiesSteps(Given given, Actual actual)
        {
            _given = given;
            _actual = actual;
        }

        [Given(@"JObject has properties '(.*)'")]
        public void GivenJObjectHasProperties(string propertyNames)
        {
            _given.JObject = new JObject();

            foreach (var propertyName in SplitPropertyNames(propertyNames))
            {
                _given.JObject.Add(propertyName, "dummy value");
            }
        }

        private static IEnumerable<string> SplitPropertyNames(string propertyNames)
        {
            return propertyNames.Split(',').Select(p => p.Trim());
        }

        [When(@"I call JObjectExtensions\.HasProperties\(jObject, '(.*)'\)")]
        public void WhenICallJObjectExtensions_HasPropertiesJObject(string propertyNames)
        {
            _actual.Result = _given.JObject.HasProperties(SplitPropertyNames(propertyNames).ToArray());
        }

        [Then(@"the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            _actual.Result.As<bool>().Should().BeTrue();
        }

        [Then(@"the result should be false")]
        public void ThenTheResultShouldBeFalse()
        {
            _actual.Result.As<bool>().Should().BeFalse();
        }
    }
}