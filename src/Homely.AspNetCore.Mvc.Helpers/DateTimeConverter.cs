using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Homely.AspNetCore.Mvc.Helpers
{
    // based on REF: https://stackoverflow.com/a/58103218/30674	
    // Also info on default behavior for DateTimes with System.Text.Json: https://docs.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support	
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string _dateTimeFormat;

        public DateTimeConverter(string dateTimeFormat)
        {
            _dateTimeFormat = dateTimeFormat;
        }

        /// <inheritdoc/>	
        public override DateTime Read(ref Utf8JsonReader reader,
                                      Type typeToConvert,
                                      JsonSerializerOptions options)
        {
            var text = reader.GetString();

            if (string.IsNullOrWhiteSpace(text) )
            {
                throw new Exception("Utf8JsonReader contained null/empty/whitespace content. Unble to parse as a DateTime."); 
            }

            return DateTime.Parse(text);
        }

        /// <inheritdoc/>	
        public override void Write(Utf8JsonWriter writer,
                                   DateTime value,
                                   JsonSerializerOptions options) => writer.WriteStringValue(value.ToString(_dateTimeFormat));
    }
}
