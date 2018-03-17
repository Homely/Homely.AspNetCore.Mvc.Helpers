using Homely.AspNetCore.Mvc.Helpers.Models;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ValidationErrorTests : TestSetup
    {
        // Controller manually threw a ValidationException.
        [Fact]
        public async Task GivenAGetRequestWhichManuallyThrowsAValidationError_ValidationError_ReturnsAnHttp400()
        {
            // Arrange.
            var errors = new
            {
                errors = new List<ApiError>
                {
                    new ApiError
                    {
                        Message = "Validation failed: \r\n -- Age is not valid.\r\n -- no person Id was provided.\r\n -- No person name was provided."
                    }
                }
            };

            // Act.
            var response = await Client.GetAsync("/test/validationerror");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.Content.ShouldLookLike(errors);
        }

        // Route argument value is invalid. Passed in a string, expects an int.
        [Fact]
        public async Task GivenAGetRequestWithABadInputModel_Error_ReturnsAnHttp400()
        {
            // Arrange.
            var routeValue = "aaa";
            var errors = new
            {
                errors = new List<ApiError>
                {
                    new ApiError
                    {
                        Key = "id",
                        Message = $"The value '{routeValue}' is not valid."
                    }
                }
            };

            // Act.
            var response = await Client.GetAsync($"/test/validationerror/{routeValue}");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.Content.ShouldLookLike(errors);
        }
    }
}
