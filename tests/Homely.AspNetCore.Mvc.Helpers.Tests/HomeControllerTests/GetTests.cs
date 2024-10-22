using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.HomeControllerTests
{
    public class GetTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetTests(CustomWebApplicationFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenARequest_Get_ReturnsAnHttp200()
        {
            // Arrange.
            const string banner = "pew pew";
            var currentAssembly = Assembly.GetExecutingAssembly();
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddAHomeController(services, banner, currentAssembly);
                });
            }).CreateClient();

            // Act.
            var response = await client.GetAsync("/");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            var text = await response.Content.ReadAsStringAsync();
            text.ShouldStartWith(banner); // Banner ASCII art/text.

            var buildDate = System.IO.File.GetLastWriteTime(currentAssembly.Location).ToString("F");

            text.ShouldContain(buildDate);
        }
    }
}
