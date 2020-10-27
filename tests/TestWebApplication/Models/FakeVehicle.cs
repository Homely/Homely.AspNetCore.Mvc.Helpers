using System;
using System.ComponentModel.DataAnnotations;

namespace TestWebApplication.Models
{
    public class FakeVehicle
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public ColourType Colour { get; set; }

        /// <summary>
        /// Optional: VIN of the vehicle.
        /// </summary>
        /// <remarks>Optional - might not always have this data.</remarks>
        public string VIN { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public BaseClass SomeBaseClass { get; set; }
        public BaseClass AnotherBaseClass1 { get; set; }
        public BaseClass AnotherBaseClass2 { get; set; }
        public DerivedClass SomeDerivedClass { get; set; }
        public AnotherDerivedClass AnotherDerivedClass { get; set; }
    }
}
