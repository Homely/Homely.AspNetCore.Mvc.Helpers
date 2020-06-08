using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class DynamicErrorTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public DynamicErrorTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

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

            var client = _factory.CreateClient();

            // TODO: Set IsDevelopment() so we can proove that both error messages are different.

            // Act.
            var response1 = await _factory.CreateClient().GetAsync(route);
            var response2 = await _factory.CreateClient().GetAsync(route);

            // Assert.
            response1.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            response2.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

            await response1.Content.ShouldHaveSameProblemDetails(error);
            await response2.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
