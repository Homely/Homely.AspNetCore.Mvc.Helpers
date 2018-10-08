using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Shouldly;
using System;
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
            // TODO: Fix
            throw new NotImplementedException();

            // Arrange.

            // Act.
            var response = await Client.GetAsync("/test/error");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            //await response.Content.ShouldLookLike(DefaultErrorViewModel);
        }

        [Fact]
        public async Task GivenAGetRequestAndACustomExceptionFunction_Error_ReturnsAnHttp426()
        {
            // TODO: Fix
            throw new NotImplementedException();

            // Arrange.

            // We want to use our own startup stuff so we can define our error callback.
            //var server = new TestServer(new WebHostBuilder().UseStartup<TestStartupWithCustomJsonExceptionPageCustomHandler>());
            //var client = server.CreateClient();

            //var errors = new ValidationProblemDetails();

            //// Act.
            //var response = await client.GetAsync("/test/error");

            //// Assert.
            //response.StatusCode.ShouldBe(HttpStatusCode.UpgradeRequired);
            //await response.Content.ShouldLookLike(errors);

            // blew might not be needed.
            //// We can't use the normal model<-->model compare because the response contains
            //// a massive stack trace now (by design) and that's too hard to setup. So we'll
            //// do a simple property-by-property check.
            //var apiErrorsJson = await response.Content.ReadAsStringAsync();
            //var apiErrorResult = JsonConvert.DeserializeObject<ApiErrorResult>(apiErrorsJson);
            //apiErrorResult.Errors.ShouldLookLike(errors);
            //apiErrorResult.StackTrace.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GivenAGetRequestAndIncludeStackTrace_Error_ReturnsAnHttp500()
        {
            // TODO: Fix
            throw new NotImplementedException();

            //// Arrange.

            //// We want to use our own startup stuff so we can define our error callback.
            //var server = new TestServer(new WebHostBuilder().UseStartup<TestStartupWithCustomJsonExceptionPageIncludeStackTrace>());
            //var client = server.CreateClient();

            //// Act.
            //var response = await client.GetAsync("/test/error");

            //// Assert.
            //response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

            //// We can't use the normal model<-->model compare because the response contains
            //// a massive stack trace now (by design) and that's too hard to setup. So we'll
            //// do a simple property-by-property check.
            //var apiErrorsJson = await response.Content.ReadAsStringAsync();

            //// TODO: FIx.
            //// var apiErrorResult = JsonConvert.DeserializeObject<ApiErrorResult>(apiErrorsJson);
            ////apiErrorResult.Errors.ShouldLookLike(DefaultErrorViewModel.Errors);
            ////apiErrorResult.StackTrace.ShouldNotBeNullOrWhiteSpace();
        }
    }
}
