using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.AuthorizedControllerTests
{
    public class GetTests : TestSetup
    {
        [Fact]
        public async Task GivenAnUnauthorizedRequest_Get_ReturnsAnHttp401()
        {
            // Arrange.
            
            // Act.
            var response = await Client.GetAsync("/authorized");

            // Assert.
            response.IsSuccessStatusCode.ShouldBeFalse();
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GivenAnAuthorizedRequest_Get_ReturnsAnHttp401()
        {
            // Arrange.

            var request = TestServer.CreateRequest("/authorized");
            request.AddHeader("Authorization", "bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6MSwibmFtZSI6IkhhbiBTb2xvIn0.NVPIbBSctW_tmxSMAgNdBveal6iuDRNuEuse-ENSJVU");

            // Act.
            var response = await request.GetAsync();

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        }
    }
}
