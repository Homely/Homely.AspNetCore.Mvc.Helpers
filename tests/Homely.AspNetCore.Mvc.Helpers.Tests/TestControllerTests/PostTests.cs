using Homely.AspNetCore.Mvc.Helpers.ViewModels;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class PostTests : TestSetup
    {
        [Fact]
        public async Task GivenPostingAValidModel_Post_ReturnsAndHttp201()
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
            var response = await Client.PostAsync("/test", content);

            // Assert.
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.Headers.Location.ShouldBe(new System.Uri("http://localhost/test/5"));
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GivenPostingAnInvalidModel_Post_ReturnsAnHttp400()
        {
            // Arrange.
            const string badColour = "pewpew";
            var expectedResponseContent = TestHelpers.CreateAnApiError("Colour",
                $"The value '{badColour}' is not valid for Colour.");
            var fakeVehicle = FakeVehicleHelpers.CreateAFakeVehicle();
            var formData = new Dictionary<string, string>
            {
                { "Name", fakeVehicle.Name },
                { "RegistrationNumber", fakeVehicle.RegistrationNumber },
                { "Colour", badColour }
            };

            var content = new FormUrlEncodedContent(formData);

            // Act.
            var response = await Client.PostAsync("/test", content);

            // Assert.  
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<ErrorViewModel>(responseContent);
            responseModel.ShouldLookLike(expectedResponseContent);
        }
    }
}
