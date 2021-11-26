using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.HomeControllerTests
{
    public class ExceptionTestTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public ExceptionTestTests(CustomWebApplicationFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenARequest_ExceptionTests_ReturnsAnHttp500()
        {
            // Arrange.
            var expectedError = new ProblemDetails
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
            var error = await JsonSerializer.DeserializeAsync<ProblemDetails>(await response.Content.ReadAsStreamAsync());

            // We can't check the TraceId because it's different with each HTTP call.
            error?.Type.ShouldBe(expectedError.Type);
            error?.Title.ShouldBe(expectedError.Title);
            error?.Status.ShouldBe(expectedError.Status);
        }
    }
}
