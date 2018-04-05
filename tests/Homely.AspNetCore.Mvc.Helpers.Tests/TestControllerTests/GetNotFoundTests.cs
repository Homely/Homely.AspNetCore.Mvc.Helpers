using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class GetNotFoundTests : TestSetup
    {
        [Fact]
        public async Task GivenABadRoute_Get_ReturnsAnHttp400()
        {
            // Arrange.
            const int id = int.MaxValue;
            var expectedFakeVehicle = FakeVehicleHelpers.CreateAFakeVehicle(1);
            
            // Act.
            var response = await Client.GetAsync($"/test/{id}");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            var content = await response.Content.ReadAsStringAsync();
            content.ShouldNotBeNullOrWhiteSpace();
        }
    }
}
