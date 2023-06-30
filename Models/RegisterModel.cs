using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PowerPlant.Models
{
    public class RegisterModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
