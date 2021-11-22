using System;
using System.Text;
using System.Text.Json;
using Shouldly;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public class ReadTests
    {
        private static Utf8JsonReader FakeReader(string text)
        {
            var json = JsonSerializer.Serialize(text);
            ReadOnlySpan<byte> utf8Bom = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(utf8Bom);

            // First char (in the reader) is a token == None because we're at the start.	
            // So we need to move to the start of the actual json data.	
            // REF: https://stackoverflow.com/a/59039551/30674	
            reader.Read();

            return reader;
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void GivenAEmptyOrWhitespaceDateTime_Read_ThrowsAnException(string dateTime)
        {
            // Arrange.
            var converter = new DateTimeConverter("yyyy"); // dateTimeFormat is ignored, in this Read(..) method.	
            
            // Act.
            var exception = Should.Throw<Exception>(() =>
            {
                var reader = FakeReader(dateTime);

                converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());
            });

            // Assert.
            exception.ShouldNotBeNull();
            exception.Message.ShouldBe("Utf8JsonReader contained null/empty/whitespace content. Unble to parse as a DateTime.");
        }

        [Fact]
        public void GivenSomeTextThatIsNotADateTime_Read_ThrowsAnException()
        {
            // Arrange.
            var converter = new DateTimeConverter("yyyy"); // dateTimeFormat is ignored, in this Read(..) method.	
            const string badDateTime = "abcde";

            // Act.
            var exception = Should.Throw<FormatException>(() =>
            {
                var reader = FakeReader(badDateTime); // Not a valid DateTime.

                converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());
            });

            // Assert.
            exception.ShouldNotBeNull();
            exception.Message.ShouldStartWith($"The string '{badDateTime}' was not recognized as a valid DateTime.");
        }

        [Fact]
        public void GivenAValidDateTimeString_Read_ReturnsAValidDateTime()
        {
            // Arrange.	
            var converter = new DateTimeConverter("yyyy"); // dateTimeFormat is ignored, in this Read(..) method.	

            var dateTime = new DateTime(2000, 1, 2, 3, 4, 5);
            var reader = FakeReader(dateTime.ToString("s"));

            // Act.	
            var result = converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());

            // Assert.	
            result.Ticks.ShouldBe(dateTime.Ticks);
        }
    }
}
