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
    }
}
