using PowerPlant.Entities;
using PowerPlant.Models;

namespace PowerPlant.Services
{
    public interface ISolarPowerPlantService
    {
        public Task CreateOrUpdate(SolarPowerPlantModel model);

        public Task<SolarPowerPlant> GetById(int id);

        public Task<List<SolarPowerPlant>> GetAll();

        public Task Delete(int id);

        public Task<List<TimeseriesData>> GetTimeSeriesByFilters(GetByFilterModel model);
    }
}
