using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class GetTests : TestSetup
    {
        [Fact]
        public async Task GivenAValidId_Get_ReturnsAnHttp200()
        {
            // Arrange.
            const int id = 1;
            var expectedFakeVehicle = FakeVehicleHelpers.CreateAFakeVehicle(1);

            // Act.
            var response = await Client.GetAsync($"/test/{id}");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            await response.Content.ShouldLookLike(expectedFakeVehicle);
        }

        [Fact]
        public async Task GivenAnInvalidId_Get_ReturnsAnHttp404()
        {
            // Arrange.
            const int id = int.MaxValue;
            var error = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound
            };

            // Act.
            var response = await Client.GetAsync($"/test/{id}");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            await response.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
