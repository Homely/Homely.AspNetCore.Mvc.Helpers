using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using TestWebApplication.Models;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ModelBindingTests : TestSetup
    {
        [Fact]
        public async Task GivenAValidModelBind_Get_ReturnsAnHttp200()
        {
            // Arrange & Act.
            var response = await Client.GetAsync($"/test/modelbinding/{ColourType.GreenAndPink}");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GivenABadModelBind_Get_ReturnsAnHttp400()
        {
            // Arrange.
            var error = new ValidationProblemDetails
            {
                //Type = "https://httpstatuses.com/400",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details.",
                Instance = "/test/modelbinding/pewpew"
            };
            error.Errors.Add("colour", new[] { "The value 'pewpew' is not valid." });

            // Act.
            var response = await Client.GetAsync($"/test/modelbinding/pewpew");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
