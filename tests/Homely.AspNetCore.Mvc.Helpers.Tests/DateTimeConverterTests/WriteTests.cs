using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.DateTimeConverterTests
{
    public class WriteTests
    {
        [Fact]
        public void GivenADateTime_Write_ReturnsAJsonString()
        {
            // Arrange.
            var converter = new DateTimeConverter("yyyy"); // Convert the DateTime to just a 'year' string representation.
            var dateTime = new DateTime(2000, 1, 2, 3, 4, 5);

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(memoryStream))
                {
                    // Act.
                    converter.Write(writer, dateTime, new JsonSerializerOptions());
                }

                // Assert.
                var result = Encoding.UTF8.GetString(memoryStream.ToArray());
                result.ShouldBe("\"2000\"");
            }
        }
    }
}
