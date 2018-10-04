using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Homely.AspNetCore.Mvc.Helpers.Helpers
{
    public static class JsonHelpers
    {
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
