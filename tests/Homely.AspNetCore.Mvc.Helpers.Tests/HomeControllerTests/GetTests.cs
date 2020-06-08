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

            // Act.
            var response = await _factory.CreateClient().GetAsync("/");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            var text = await response.Content.ReadAsStringAsync();
            text.ShouldStartWith("pew pew"); // Banner ASCII art/text.
        }
    }
}
