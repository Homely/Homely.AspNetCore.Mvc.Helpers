using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ErrorTests : TestSetup
    {
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
            var response = await Client.GetAsync("/test/error");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            await response.Content.ShouldLookLike(error);
        }

        //[Fact]
        //public async Task GivenAGetRequestAndIncludeStackTrace_Error_ReturnsAnHttp500()
        //{
        //    // Arrange.

        //    // We want to use our own startup stuff so we can define our error callback.
        //    var webHostBuilder = new WebHostBuilder().UseStartup<TestStartupWithCustomJsonExceptionPageIncludeStackTrace>();
        //    var server = new TestServer(webHostBuilder);
        //    var client = server.CreateClient();

        //    // Act.
        //    var response = await client.GetAsync("/test/error");

        //    // Assert.
        //    response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        //    // We can't use the normal model<-->model compare because the response contains
        //    // a massive stack trace now (by design) and that's too hard to setup. So we'll
        //    // do a simple property-by-property check.
        //    var apiErrorsJson = await response.Content.ReadAsStringAsync();

        //    // TODO: FIx.
        //    // var apiErrorResult = JsonConvert.DeserializeObject<ApiErrorResult>(apiErrorsJson);
        //    //apiErrorResult.Errors.ShouldLookLike(DefaultErrorViewModel.Errors);
        //    //apiErrorResult.StackTrace.ShouldNotBeNullOrWhiteSpace();
        //}
    }
}
