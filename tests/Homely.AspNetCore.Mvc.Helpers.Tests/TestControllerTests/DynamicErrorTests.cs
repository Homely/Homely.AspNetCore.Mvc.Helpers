using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class DynamicErrorTests : TestSetup
    {
        // Controller manually threw a ValidationException.
        [Fact]
        public async Task GivenTwoGetRequestWhichManuallyThrowsAnError_ValidationError_ReturnsTwoHttp500ResponsesAndTheyAreDifferent()
        {
            // Arrange.
            const string route = "/test/dynamicError";

            // Act.
            var response1 = await Client.GetAsync(route);
            var response2 = await Client.GetAsync(route);

            // Assert.
            response1.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            response2.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            
            var response1Text = await response1.Content.ReadAsStringAsync();
            var response2Text = await response2.Content.ReadAsStringAsync();
            response1.ShouldNotBe(response2);
            response1Text.ShouldNotBeNullOrWhiteSpace();
            response2Text.ShouldNotBeNullOrWhiteSpace();
        }
    }
}
