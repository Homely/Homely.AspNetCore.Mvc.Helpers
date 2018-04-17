using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class DynamicValidationErrorTests : TestSetup
    {
        // Controller manually threw a ValidationException.
        [Fact]
        public async Task GivenTwoGetRequestWhichManuallyThrowsAValidationError_ValidationError_ReturnsTwoHttp400ResponsesAndTheyAreDifferent()
        {
            // Arrange.
            const string route = "/test/dynamicValidationError";

            // Act.
            var response1 = await Client.GetAsync(route);
            var response2 = await Client.GetAsync(route);

            // Assert.
            response1.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            response2.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            
            var response1Text = await response1.Content.ReadAsStringAsync();
            var response2Text = await response2.Content.ReadAsStringAsync();
            response1.ShouldNotBe(response2);
        }
    }
}
