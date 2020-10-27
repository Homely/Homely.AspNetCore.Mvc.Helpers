using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestWebApplication.Models;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.JsonOptionsTests
{
    public class BaseClassConverter : JsonConverter<BaseClass>
    {
        public override BaseClass Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BaseClass value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is DerivedClass derivedClass)
            {
                writer.WriteString(nameof(derivedClass.SomeDerivedClassProperty), derivedClass.SomeDerivedClassProperty);
            }

            if (value is AnotherDerivedClass anotherDerivedClass)
            {
                writer.WriteString(nameof(anotherDerivedClass.AnotherDerivedClassProperty), anotherDerivedClass.AnotherDerivedClassProperty);
            }

            writer.WriteString(nameof(value.SomeAbstractClassProperty), value.SomeAbstractClassProperty);
            writer.WriteString(nameof(value.SomeBaseClassProperty), value.SomeBaseClassProperty);

            writer.WriteEndObject();
        }
    }
}
