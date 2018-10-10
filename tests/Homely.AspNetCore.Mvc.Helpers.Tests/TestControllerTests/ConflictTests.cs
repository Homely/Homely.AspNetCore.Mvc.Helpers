using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ConflictTests : TestSetup
    {
        [Fact]
        public async Task GivenAValidId_Get_ReturnsAnHtt409()
        {
            // Arrange.
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Agent was already modified.",
                Status = StatusCodes.Status409Conflict,
                Detail = "agent was already modified after you retrieved the latest data. So you would then override the most recent copy. As such, you will need to refresh the page (to get the latest data) then modify that, if required.",
                Instance = "/test/conflict"
            };

            // Act.
            var response = await Client.GetAsync("/test/conflict");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
            await response.Content.ShouldLookLike(error);
        }
    }
}
