using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class GetNotFoundTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetNotFoundTests(CustomWebApplicationFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenABadRoute_Get_ReturnsAnHttp404()
        {
            // Arrange.
            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.io/404",
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound
            };
            // Act.
            var response = await _factory.CreateClient().GetAsync($"/test/pewpewpewpew");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            await response.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
