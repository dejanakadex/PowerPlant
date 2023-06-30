using Microsoft.EntityFrameworkCore;
using PowerPlant.Entities;

namespace PowerPlant.Infrastructure
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<User> Users { get; set; }

        public DbSet<SolarPowerPlant> SolarPowerPlants { get; set; }

        public DbSet<TimeseriesData> TimeseriesData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SolarPowerPlant>()
                .HasMany(e => e.Timeseries)
                .WithOne(e => e.SolarPowerPlant)
                .HasForeignKey(e => e.SolarPowerPlantID)
                .HasPrincipalKey(e => e.Id);


            modelBuilder.Entity<User>().HasData(
                //pass: 123
                new User() { Id = 1, FirstName = "Dejan", LastName = "Denadija", Username = "ddena", PasswordHash = "$2a$12$zWboW3IjfqmsALYWqqxsF.MVZVQqtM8h6r2tslDk09V.4ymdzmRru", Salt = "$2a$12$zWboW3IjfqmsALYWqqxsF." } 
                );

            modelBuilder.Entity<SolarPowerPlant>().Property(a => a.Latitude).HasPrecision(18, 9);
            modelBuilder.Entity<SolarPowerPlant>().Property(a => a.Longitude).HasPrecision(18, 9);


            var solarPowerPlantData = new List<SolarPowerPlant>() 
            {
                new SolarPowerPlant() { Id = 1, InstallationDate = DateTime.UtcNow.AddDays(-1), InstalledPower = 100, Name = "FirstPlant", Latitude = 15, Longitude = 23 },
                new SolarPowerPlant() { Id = 2, InstallationDate = DateTime.UtcNow.AddDays(-2), InstalledPower = 200, Name = "SecondPlant", Latitude = 16, Longitude = 24 },
                new SolarPowerPlant() { Id = 3, InstallationDate = DateTime.UtcNow.AddDays(-3), InstalledPower = 300, Name = "ThirdPlant", Latitude = 17, Longitude = 25 }
            };

            modelBuilder.Entity<SolarPowerPlant>().HasData(solarPowerPlantData);


            modelBuilder = SeedTimeSeries(modelBuilder, solarPowerPlantData);

            base.OnModelCreating(modelBuilder);
        }

        private ModelBuilder SeedTimeSeries(ModelBuilder modelBuilder, List<SolarPowerPlant> solarPowerPlantData)
        {
            var now = DateTime.UtcNow;

            var startDate = now.AddDays(-1);

            var listTimeSeries = new List<TimeseriesData>();

            Random rnd = new Random();
            int i = 1;

            foreach (var solar in solarPowerPlantData)
            {
                while(startDate < now.AddDays(1))
                {
                    if (startDate < now)
                    {
                        listTimeSeries.Add(new Entities.TimeseriesData()
                        {
                            DateTimeCreated = startDate,
                            Id = i,
                            SolarPowerPlantID = solar.Id,
                            Type = TimeseriesTypeEnum.Real,
                            Value = rnd.Next(100, 10000)
                        });

                        i++;
                    }

                    listTimeSeries.Add(new Entities.TimeseriesData()
                    {
                        DateTimeCreated = startDate,
                        Id = i,
                        SolarPowerPlantID = solar.Id,
                        Type = TimeseriesTypeEnum.Forecasted,
                        Value = rnd.Next(100, 10000)
                    });

                    i++;
                    startDate = startDate.AddMinutes(15);
                }

                startDate = now.AddDays(-1);
            }

            modelBuilder.Entity<TimeseriesData>().HasData(listTimeSeries);

            return modelBuilder;
        }

    }
}
