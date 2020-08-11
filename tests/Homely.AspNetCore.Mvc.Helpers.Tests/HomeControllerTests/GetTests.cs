using Homely.AspNetCore.Mvc.Helpers.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.HomeControllerTests
{
    public class GetTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public GetTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenARequest_Get_ReturnsAnHttp200()
        {
            // Arrange.
            const string banner = "pew pew";
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddAHomeController(services, banner);
                });
            }).CreateClient();

            // Act.
            var response = await client.GetAsync("/");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            var text = await response.Content.ReadAsStringAsync();
            text.ShouldStartWith(banner); // Banner ASCII art/text.
        }
    }
}
