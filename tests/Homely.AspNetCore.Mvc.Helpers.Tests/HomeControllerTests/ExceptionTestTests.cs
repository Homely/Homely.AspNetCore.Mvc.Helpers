using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.HomeControllerTests
{
    public class ExceptionTestTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public ExceptionTestTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenARequest_ExceptionTests_ReturnsAnHttp500()
        {
            // Arrange.
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError
            };

            // Act.
            var response = await _factory.CreateClient().GetAsync("/exceptionTest");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeFalse();
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            await response.Content.ShouldLookLike(error);
        }
    }
}
