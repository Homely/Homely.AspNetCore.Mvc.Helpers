using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class DynamicErrorTests : TestSetup
    {
        // Controller manually threw a ValidationException.
        [Fact]
        public async Task GivenTwoGetRequestsWhichManuallyThrowsAnError_ValidationError_ReturnsTwoHttp500ResponsesAndTheyAreDifferent()
        {
            // Arrange.
            const string route = "/test/dynamicError";
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError
            };

            // TODO: Set IsDevelopment() so we can proove that both error messages are different.

            // Act.
            var response1 = await Client.GetAsync(route);
            var response2 = await Client.GetAsync(route);

            // Assert.
            response1.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            response2.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

            await response1.Content.ShouldLookLike(error);
            await response2.Content.ShouldLookLike(error);
        }
    }
}
