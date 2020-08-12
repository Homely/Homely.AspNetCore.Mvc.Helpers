using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestWebApplication.Models;
using TestWebApplication.Repositories;
using Xunit;

namespace Homely.AspNetCore.Mvc.Helpers.Tests.TestControllerTests
{
    public class PostTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _factory;

        public PostTests(TestFixture factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Fact]
        public async Task GivenPostingAValidModel_Post_ReturnsAndHttp201()
        {
            // Arrange.
            var fakeVehicle = FakeVehicleHelpers.CreateAFakeVehicle();
            var currentFakeVehicleCount = StubbedFakeVehicleRepository.CreateAFakeVehicleRepository()
                                                                      .Index()
                                                                      .Count();
            var formData = new Dictionary<string, string>
            {
                { nameof(FakeVehicle.Id), fakeVehicle.Id.ToString() },
                { nameof(FakeVehicle.Name), fakeVehicle.Name },
                { nameof(FakeVehicle.RegistrationNumber), fakeVehicle.RegistrationNumber },
                { nameof(FakeVehicle.Colour), fakeVehicle.Colour.ToString() }
            };

            var content = new FormUrlEncodedContent(formData);

            // Act.
            var response = await _factory.CreateClient().PostAsync("/test", content);

            // Assert.
            response.EnsureSuccessStatusCode();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.Headers.Location.ShouldBe(new Uri($"http://localhost/test/{currentFakeVehicleCount + 1 }"));
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GivenPostingAnInvalidModel_Post_ReturnsAnHttp400()
        {
            // Arrange.
            const string badColour = "pewpew";
            var fakeVehicle = FakeVehicleHelpers.CreateAFakeVehicle();
            var formData = new Dictionary<string, string>
            {
                { nameof(FakeVehicle.Id), fakeVehicle.Id.ToString() },
                { nameof(FakeVehicle.Name), fakeVehicle.Name },
                { nameof(FakeVehicle.RegistrationNumber), fakeVehicle.RegistrationNumber },
                { nameof(FakeVehicle.Colour), badColour }
            };
            var content = new FormUrlEncodedContent(formData);

            var error = new ValidationProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Please refer to the errors property for additional details.",
                Instance = "/test"
            };
            error.Errors.Add("Colour", new[] { "The value 'pewpew' is not valid for Colour." });

            // Act.
            var response = await _factory.CreateClient().PostAsync("/test", content);

            // Assert.  
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            await response.Content.ShouldHaveSameProblemDetails(error);
        }
    }
}
