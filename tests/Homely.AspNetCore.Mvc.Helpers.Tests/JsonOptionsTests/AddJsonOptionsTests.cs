using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.JsonOptionsTests
{
    public class AddJsonOptionsTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public AddJsonOptionsTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public static TheoryData<int, string, string> Data => new TheoryData<int, string, string>
        {
            { 1, null, "2000-01-02T03:04:05" }, // Default formatter with no milliseconds.
            { 2, null, "2000-01-02T03:04:05.666" }, // Default formatter with milliseconds.
            { 3, null, "2000-01-02T03:04:05.001" }, // Default formatter with milliseconds.
            { 1, "", "2000-01-02T03:04:05" }, // Default formatter.
            { 1, "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ", "2000-01-02T03:04:05" }, // Custom formatter with no milliseconds.
            { 1, "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ", "2000-01-02T03:04:05.000" }, // Custom formatter with milliseconds.
            { 2, "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffZ", "2000-01-02T03:04:05.666" } // Custom formatter with milliseconds.
        };

        [Theory]
        [MemberData(nameof(Data))]
        public async Task GivenSomeModel_Get_ReturnsASyntaxCorrectJsonText(int id, string dateTimeFormat, string expectedDateTime)
        {
            // Arrange.
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddDefaultJsonOptions(dateTimeFormat: dateTimeFormat);
                });
            }).CreateClient();

            // Act.
            var response = await client.GetAsync($"/test/{id}");

            // Assert.
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.ShouldContain("id", Case.Sensitive); // Camel case check.
            responseBody.ShouldNotContain("vin", Case.Insensitive); // IgnoreNullValues check. 
            responseBody.ShouldContain("Grey", Case.Sensitive); // Enum converter check.
            responseBody.ShouldContain($"{{{Environment.NewLine}  \""); // WriteIndented check.
            responseBody.ShouldContain(expectedDateTime);
        }
    }
}
