using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.SwaggerTests
{
    public class CustomSwaggerTests : TestSetup
    {
        [Fact]
        public async Task GivenASwaggerUiRequest_Get_ReturnsAnHttp200()
        {
            // Arrange.

            // Act.
            var response = await Client.GetAsync("/swagger/index.html");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenASwaggerRequest_Get_ReturnsAnHttp200()
        {
            // Arrange.

            // Act.
            var response = await Client.GetAsync("/swagger/v2/swagger.json");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
            response.Content.Headers.ContentType.CharSet.ShouldBe("utf-8");
        }
    }
}
