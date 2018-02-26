using TestWebApplication.Models;

namespace Homely.AspNetCore.Mvc.Helpers.Tests
{
    internal static class FakeVehicleHelpers
    {
        internal static FakeVehicle CreateAFakeVehicle(int id = 0,
                                                       string name = "Tesla",
                                                       string registrationNumber = "PewPew123",
                                                       ColourType colour = ColourType.Grey)
        {
            return new FakeVehicle
            {
                Id = id,
                Name = name,
                RegistrationNumber = registrationNumber,
                Colour = colour
            };
        }
    }
}
