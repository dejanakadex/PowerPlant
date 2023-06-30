using System.ComponentModel.DataAnnotations;
using PowerPlant.Entities;

namespace PowerPlant.Models
{
    public class SolarPowerPlantModel
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        [Required]
        public int InstalledPower { get; set; }

        [Required]
        public DateTime InstallationDate { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }
    }
}
