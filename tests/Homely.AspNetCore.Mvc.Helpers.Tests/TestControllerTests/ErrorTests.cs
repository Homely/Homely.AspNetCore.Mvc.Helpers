using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ErrorTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public ErrorTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

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
            var response = await _factory.CreateClient().GetAsync("/test/error");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            await response.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
