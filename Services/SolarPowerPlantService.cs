using Microsoft.EntityFrameworkCore;
using PowerPlant.Entities;
using PowerPlant.Infrastructure;
using PowerPlant.Models;

namespace PowerPlant.Services
{
    public class SolarPowerPlantService : ISolarPowerPlantService
    {
        private readonly DataContext _dataContext;
        private readonly ISeedService _seedService;
        public SolarPowerPlantService(DataContext dataContext, ISeedService seedService)
        {
            _dataContext = dataContext;
            _seedService = seedService;
        }

        public async Task CreateOrUpdate(SolarPowerPlantModel model)
        {
            if (model.Id.HasValue)
            {
                var spp = await _dataContext.SolarPowerPlants.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

                if (spp == null)
                {
                    throw new AppException($"Solar power plant with id: {model.Id} dose not exist.");
                }

                spp.Longitude = model.Longitude ?? spp.Longitude;
                spp.Latitude = model.Latitude ?? spp.Latitude;
                spp.InstalledPower = model.InstalledPower == default(int) ? spp.InstalledPower : model.InstalledPower;
                spp.InstallationDate = model.InstallationDate == default(DateTime) ? spp.InstallationDate : model.InstallationDate;
                spp.Name = model.Name ?? spp.Name;

                _dataContext.Update(spp);
                await _dataContext.SaveChangesAsync();

                return;
            }

            var nspp = new SolarPowerPlant()
            {
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                InstalledPower = model.InstalledPower,
                InstallationDate = model.InstallationDate,
                Name = model.Name
            };

            var newPlant = await _dataContext.AddAsync(nspp);
            await _dataContext.SaveChangesAsync();

            await _seedService.SeedNewPowerPlant(newPlant.Entity.Id);
        }

        public async Task<SolarPowerPlant> GetById(int id)
        {
            var spp = await _dataContext.SolarPowerPlants.Where(x => x.Id == id).FirstOrDefaultAsync();

            return spp ?? throw new AppException($"Solar power plant with id: {id} dose not exist.");
        }

        public async Task<List<SolarPowerPlant>> GetAll()
        {
            var spp = await _dataContext.SolarPowerPlants.ToListAsync();

            return spp ?? throw new AppException($"No solar power plants.");
        }

        public async Task Delete(int id)
        {
            var tsd = await _dataContext.TimeseriesData.Where(x => x.SolarPowerPlantID == id).ToListAsync();
            var spp = await _dataContext.SolarPowerPlants.Where(x => x.Id == id).FirstOrDefaultAsync();

            if(spp == null)
            {
                throw new AppException($"Solar power plant with id: {id} dose not exist.");
            }

            _dataContext.RemoveRange(tsd);
            _dataContext.Remove(spp);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<List<TimeseriesData>> GetTimeSeriesByFilters(GetByFilterModel model)
        {
            IQueryable<TimeseriesData> query = _dataContext.TimeseriesData.Where(
                x => x.SolarPowerPlantID == model.SolarPowerPlantId
            && (model.TimeseriesType == null || x.Type == model.TimeseriesType.Value)
            && ((!model.StartDateTimeUtc.HasValue && !model.EndDateTimeUtc.HasValue) 
                || model.StartDateTimeUtc!.Value <= x.DateTimeCreated && model.EndDateTimeUtc!.Value >= x.DateTimeCreated)
            );

            if (model.Granularity == GranularityEnum.Hour1Granularity)
            {
                query = query.GroupBy(x => new
                {
                    x.Type,
                    x.DateTimeCreated.Year,
                    x.DateTimeCreated.Month,
                    x.DateTimeCreated.Day,
                    x.DateTimeCreated.Hour
                })
                .Select(g => new TimeseriesData
                {
                    Type = g.First().Type,
                    SolarPowerPlantID = g.First().SolarPowerPlantID,
                    DateTimeCreated = g.First().DateTimeCreated,
                    Value = g.Sum(pr => pr.Value)
                });
            }

            var res = await query.OrderBy(pr => pr.DateTimeCreated).ToListAsync();

            return res;
        }

    }
}
