using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ErrorTests : TestSetup
    {
        [Fact]
        public async Task GivenAGetRequest_Error_ReturnsAnHttp500()
        {
            // Arrange.
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError
            };

            // Act.
            var response = await Client.GetAsync("/test/error");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            await response.Content.ShouldLookLike(error);
        }
    }
}
