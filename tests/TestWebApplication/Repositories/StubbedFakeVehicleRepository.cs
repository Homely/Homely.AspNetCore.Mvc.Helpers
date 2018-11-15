using TestWebApplication.Models;

namespace TestWebApplication.Repositories
{
    public static class StubbedFakeVehicleRepository
    {
        public static FakeVehicleRepository CreateAFakeVehicleRepository()
        {
            var stubbedFakeVehicleRepository = new FakeVehicleRepository();
            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 1,
                Name = "Name1",
                RegistrationNumber = "RegistrationNumber1",
                Colour = ColourType.Grey
            });
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
                Colour = ColourType.GreenAndPink,
                Name = "Name4",
                RegistrationNumber = "RegistrationNumber4"
            });

            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 5,
                Colour = ColourType.Green,
                Name = "Name5",
                RegistrationNumber = "RegistrationNumber5"
            });

            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 6,
                Colour = ColourType.BlackAndYellow,
                Name = "Name6",
                RegistrationNumber = "RegistrationNumber6"
            });
            return stubbedFakeVehicleRepository;
        }
    }
}
