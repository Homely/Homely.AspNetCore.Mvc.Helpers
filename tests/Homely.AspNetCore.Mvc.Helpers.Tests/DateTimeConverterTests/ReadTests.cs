using System;
using System.Text;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public class ReadTests
    {
        [Fact]
        public void GivenAValidDateTimeString_Read_ReturnsAValidDateTime()
        {
            // Arrange.	
            var converter = new DateTimeConverter("yyyy"); // dateTimeFormat is ignored, in this Read(..) method.	

            var dateTime = new DateTime(2000, 1, 2, 3, 4, 5);
            var json = JsonSerializer.Serialize(dateTime);
            ReadOnlySpan<byte> utf8Bom = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(utf8Bom);

            // First char (in the reader) is a token == None because we're at the start.	
            // So we need to move to the start of the actual json data.	
            // REF: https://stackoverflow.com/a/59039551/30674	
            reader.Read();

            // Act.	
            var result = converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());

            // Assert.	
            result.Ticks.ShouldBe(dateTime.Ticks);
        }
    }
}
