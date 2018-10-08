using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
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
            var error = new ValidationProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            };
            error.Errors.Add("someProperty", new[] { "This property failed validation." });

            // Act.
            var response = await Client.GetAsync("/test/validationerror");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.Content.ShouldLookLike(error);
        }
    }
}
