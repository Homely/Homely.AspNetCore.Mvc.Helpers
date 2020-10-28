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
                Colour = ColourType.Grey,
                CreatedOn = new System.DateTime(2000, 1, 2, 3, 4, 5, 0), // Testing ZERO milliseconds.
                SomeBaseClass = new BaseClass
                {
                    SomeAbstractClassProperty = "SBC_Abstract_Property",
                    SomeBaseClassProperty = "SBC_Base_Property"
                },
                AnotherBaseClass1 = new DerivedClass
                {
                    SomeAbstractClassProperty = "ABC1_Abstract_Property",
                    SomeBaseClassProperty = "ABC1_Base_Property",
                    SomeDerivedClassProperty = "ABC1_Derived_Property"
                },
                AnotherBaseClass2 = new AnotherDerivedClass
                {
                    SomeAbstractClassProperty = "ABC2_Abstract_Property",
                    SomeBaseClassProperty = "ABC2_Base_Property",
                    AnotherDerivedClassProperty = "ABC2_AnotherDerived_Property",
                },
                SomeDerivedClass = new DerivedClass
                {
                    SomeAbstractClassProperty = "SDC_Abstract_Property",
                    SomeBaseClassProperty = "SDC_Base_Property",
                    SomeDerivedClassProperty = "SDC_Derived_Property"
                },
                AnotherDerivedClass = new AnotherDerivedClass
                {
                    SomeAbstractClassProperty = "ADC_Abstract_Property",
                    SomeBaseClassProperty = "ADC_Base_Property",
                    AnotherDerivedClassProperty = "ADC_AnotherDerived_Property",
                }
            });

            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 2,
                Colour = ColourType.Grey,
                Name = "Name2",
                RegistrationNumber = "RegistrationNumber2",
                CreatedOn = new System.DateTime(2000, 1, 2, 3, 4, 5, 666) // Testing with milliseconds.
            });

            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 3,
                Colour = ColourType.Grey,
                Name = "Name3",
                RegistrationNumber = "RegistrationNumber3",
                CreatedOn = new System.DateTime(2000, 1, 2, 3, 4, 5, 1) // Testing with 1 (not 3) milliseconds.
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

            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 7,
                Colour = ColourType.Black,
                Name = "Name7",
                RegistrationNumber = "RegistrationNumber7"
            });

            stubbedFakeVehicleRepository.Add(new FakeVehicle
            {
                Id = 8,
                Colour = ColourType.Blue,
                Name = "Name8",
                RegistrationNumber = "RegistrationNumber8"
            });

            return stubbedFakeVehicleRepository;
        }
    }
}
