using PowerPlant.Entities;

namespace PowerPlant.Services
{
    public interface ISeedService
    {
        public Task SeedNewPowerPlant(int solarPowerPlantId);
    }
}
