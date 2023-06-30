using PowerPlant.Entities;
using PowerPlant.Infrastructure;

namespace PowerPlant.Services
{
    public class SeedService : ISeedService
    {
        private readonly DataContext _dataContext;

        public SeedService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SeedNewPowerPlant(int solarPowerPlantId)
        {
            var now = DateTime.UtcNow;

            var startDate = now.AddDays(-1);

            var listTimeSeries = new List<TimeseriesData>();

            Random rnd = new Random();

            while (startDate < now.AddDays(1))
            {
                if (startDate < now)
                {
                    listTimeSeries.Add(new Entities.TimeseriesData()
                    {
                        DateTimeCreated = startDate,
                        SolarPowerPlantID = solarPowerPlantId,
                        Type = TimeseriesTypeEnum.Real,
                        Value = rnd.Next(100, 10000)
                    });
                }

                listTimeSeries.Add(new Entities.TimeseriesData()
                {
                    DateTimeCreated = startDate,
                    SolarPowerPlantID = solarPowerPlantId,
                    Type = TimeseriesTypeEnum.Forecasted,
                    Value = rnd.Next(100, 10000)
                });

                startDate = startDate.AddMinutes(15);
            }

            await _dataContext.AddRangeAsync(listTimeSeries);
            _dataContext.SaveChanges();
        }
    }
}
