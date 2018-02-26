using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Shouldly;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TestWebApplication;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class CreateTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public CreateTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GivenPostingAValidModel_ValidateModel_ReturnsSomeJson()
        {
            // Arrange.
            var fakeVehicle = FakeVehicleHelpers.CreateAFakeVehicle();
            var formData = new Dictionary<string, string>
            {
                { "Name", fakeVehicle.Name },
                { "RegistrationNumber", fakeVehicle.RegistrationNumber },
                { "Colour", fakeVehicle.Colour.ToString() }
            };

            var content = new FormUrlEncodedContent(formData);

            // Act.
            var response = await _client.PostAsync("/test/create", content);

            // Assert.
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
            response.Headers.Location.ShouldBe(new System.Uri("http://localhost/test/11"));
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldBeNullOrWhiteSpace();
        }
    }
}
