using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class SlowDelayTests : TestSetup
    {
        [Fact]
        public async Task GivenACancellationTokenThatIsCancelled_SlowDelay_ReturnsOK()
        {
            // Arrange.
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.CancelAfter(1000 * 1);

            var error = new ProblemDetails
            {
                Type = "https://httpstatuses.com/499",
                Title = "Request was cancelled.",
                Status = 499,
                Instance = "/test/slowdelay"
            };

            // Act.
            var response = await Client.GetAsync("/test/slowdelay", cancellationToken.Token);

            // Assert.
            response.IsSuccessStatusCode.ShouldBeFalse();
            ((int)response.StatusCode).ShouldBe(499);
            await response.Content.ShouldLookLike(error);
        }

        [Fact]
        public async Task GivenACancellationTokenThatIsNotCancelled_SlowDelay_ReturnsOK()
        {
            // Arrange.
            var cancellationToken = new CancellationTokenSource();
            cancellationToken.CancelAfter(1000 * 100);

            // Act.
            var response = await Client.GetAsync("/test/slowdelay/1", cancellationToken.Token);

            // Assert.
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        }
    }
}
