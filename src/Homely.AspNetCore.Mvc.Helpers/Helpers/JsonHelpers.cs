using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Homely.AspNetCore.Mvc.Helpers.Helpers
{
    public static class JsonHelpers
    {
        /// <summary>
        /// Some consistent JSON settings.
        /// </summary>
        /// <remarks>- Camel casing.<br/>
        /// - Indented formatting.<br/>
        /// - Ignores null properties.<br/>
        /// - DateTime's are in ISO format.</br>
        /// - Enums are converted to strings (for easy reading + maintainability)</remarks>
        public static JsonSerializerSettings JsonSerializerSettings 
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat
                };

                settings.Converters.Add(new StringEnumConverter());

                return settings;
            }
        }
    }
}