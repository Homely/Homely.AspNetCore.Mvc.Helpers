using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class ConflictTests : TestSetup
    {
        [Fact]
        public async Task GivenAValidId_Get_ReturnsAnHtt409()
        {
            // Arrange & Act.
            var response = await Client.GetAsync($"/test/conflict");

            // Assert.
            response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        }
    }
}
