using System;
using Homely.AspNetCore.Mvc.Helpers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestWebApplication;
using TestWebApplication.Models;
using TestWebApplication.Repositories;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureRepositories(IServiceCollection services)
        {
            // Create our fake data.
            var stubbedFakeVehicleRepository = new FakeVehicleRepository();
            stubbedFakeVehicleRepository.Add(FakeVehicleHelpers.CreateAFakeVehicle(1));
            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 2,
                Colour = ColourType.Black,
                Name = "Name2",
                RegistrationNumber = "RegistrationNumber2"
            });
            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 3,
                Colour = ColourType.Blue,
                Name = "Name3",
                RegistrationNumber = "RegistrationNumber3"
            });
            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 4,
                Colour = ColourType.Green,
                Name = "Name4",
                RegistrationNumber = "RegistrationNumber4"
            });

            // Inject our repo.
            services.AddSingleton<IFakeVehicleRepository>(stubbedFakeVehicleRepository);
        }
    }
}
