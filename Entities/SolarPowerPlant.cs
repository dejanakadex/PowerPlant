using System.Text.Json.Serialization;

namespace PowerPlant.Entities
{
    public class SolarPowerPlant
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int InstalledPower { get; set; }

        public DateTime InstallationDate { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        [JsonIgnore]
        public virtual ICollection<TimeseriesData>? Timeseries { get; set; }
    }
}
