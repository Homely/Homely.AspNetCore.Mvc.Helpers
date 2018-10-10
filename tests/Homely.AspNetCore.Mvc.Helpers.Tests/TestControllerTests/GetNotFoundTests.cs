using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class GetNotFoundTests : TestSetup
    {
        [Fact]
        public async Task GivenABadRoute_Get_ReturnsAnHttp404()
        {
            // Arrange.
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound
            };
            // Act.
            var response = await Client.GetAsync($"/test/pewpewpewpew");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            await response.Content.ShouldLookLike(error);
        }
    }
}
