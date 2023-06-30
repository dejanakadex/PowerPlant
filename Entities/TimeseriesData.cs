using System.Text.Json.Serialization;

namespace PowerPlant.Entities
{
    public class TimeseriesData
    {
        [JsonIgnore]
        public int Id { get; set; }

        public int SolarPowerPlantID { get; set; }

        [JsonIgnore]
        public SolarPowerPlant? SolarPowerPlant { get; set; }


        public TimeseriesTypeEnum Type { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public double Value { get; set; }
    }
}
