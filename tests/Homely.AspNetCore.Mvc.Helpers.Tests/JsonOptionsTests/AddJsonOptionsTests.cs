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

        [Fact]
        public async Task GivenSomeModel_Get_ReturnsASyntaxCorrectJsonText()
        {
            // Arrange.
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddDefaultJsonOptions();
                });
            }).CreateClient();

            // Act.
            var response = await client.GetAsync("/test/1");

            // Assert.
            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.ShouldContain("id", Case.Sensitive); // Camel case check.
            responseBody.ShouldNotContain("vin", Case.Insensitive); // IgnoreNullValues check. 
            responseBody.ShouldContain("Grey", Case.Sensitive); // Enum converter check.
            responseBody.ShouldContain("{\r\n  \""); // WriteIndented check.
        }
    }
}
