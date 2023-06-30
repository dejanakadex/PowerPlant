using System.ComponentModel.DataAnnotations;
using PowerPlant.Entities;

namespace PowerPlant.Models
{
    public class GetByFilterModel
    {
        [Required]
        public int SolarPowerPlantId { get; set; }
        public TimeseriesTypeEnum? TimeseriesType { get; set; }

        public DateTime? StartDateTimeUtc { get; set; }

        public DateTime? EndDateTimeUtc { get; set; }

        public GranularityEnum Granularity { get; set; } = GranularityEnum.Minutes15Granularity;
    }
}
