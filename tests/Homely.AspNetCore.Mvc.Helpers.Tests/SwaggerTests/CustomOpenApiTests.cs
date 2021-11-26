using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.SwaggerTests
{
    public class CustomOpenApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CustomOpenApiTests(CustomWebApplicationFactory factory)
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
        public async Task GivenCustomOperatorIdSelectorsAreAutoSetupForASwaggerUiRequest_Get_ReturnsAnHttp200()
        {
            // Arrange.
            var customFactory = new CustomWebApplicationFactory();

            customFactory.WithWebHostBuilder(builder =>
            {
                builder
                    .ConfigureTestServices(services =>
                    {
                        services.AddDefaultWebApiSettings("some banner",
                            true,
                            true,
                            null,
                            null, // No customSwaggerGenerationOptions, which will then autogenerate it (which is what we are testing).
                            null);
                    });
            });

            // Act.
            var response = await customFactory.CreateClient().GetAsync("/swagger/index.html");

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

            var json = await response.Content.ReadAsStringAsync();
            json.ShouldContain("\"title\": \"Test API\"");
            json.ShouldContain("\"version\": \"v2\"");
        }
    }
}
