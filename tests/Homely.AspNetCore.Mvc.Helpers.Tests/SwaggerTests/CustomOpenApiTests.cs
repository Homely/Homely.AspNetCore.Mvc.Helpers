using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.SwaggerTests
{
    public class CustomOpenApiTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public CustomOpenApiTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenASwaggerUiRequest_Get_ReturnsAnHttp200()
        {
            // Arrange.

            // Act.
            var response = await _factory.CreateClient().GetAsync("/swagger/index.html");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenASwaggerRequest_Get_ReturnsAnHttp200()
        {
            // Arrange.

            // Act.
            var response = await _factory.CreateClient().GetAsync("/swagger/v2/swagger.json");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");
            response.Content.Headers.ContentType?.CharSet.ShouldBe("utf-8");
        }
    }
}
