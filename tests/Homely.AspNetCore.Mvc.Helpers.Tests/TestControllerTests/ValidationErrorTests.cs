using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ValidationErrorTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public ValidationErrorTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        // Controller manually threw a ValidationException.
        [Fact]
        public async Task GivenAGetRequestWhichManuallyThrowsAValidationError_ValidationError_ReturnsAnHttp400()
        {
            // Arrange.
            var error = new ValidationProblemDetails
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            };
            error.Errors.Add("someProperty", new[] { "This property failed validation." });

            // Act.
            var response = await _factory.CreateClient().GetAsync("/test/validationerror");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
