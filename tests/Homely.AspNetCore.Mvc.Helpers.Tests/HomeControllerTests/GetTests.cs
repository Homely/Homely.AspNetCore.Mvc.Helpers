using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.HomeControllerTests
{
    public class GetTests : TestSetup
    {
        [Fact]
        public async Task GivenARequest_Get_ReturnsAnHttp200()
        {
            // Arrange.

            // Act.
            var response = await Client.GetAsync("/");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            var text = await response.Content.ReadAsStringAsync();
            text.ShouldStartWith("pew pew"); // Banner ASCII art/text.
        }
    }
}
