using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Text;
using PowerPlant.Infrastructure;
using PowerPlant.Services;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    var config = builder.Configuration;

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
    builder.Host.UseNLog();

    builder.Services.Configure<Token>(builder.Configuration.GetSection("Token"));

    builder.Services.AddAuthentication().AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = config["Token:Issuer"],
            ValidAudience = config["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Token:Secret"]!))
        };
    });

    builder.Services.AddControllers();

    builder.Services.AddDbContext<DataContext>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IJwtTools, JwtTools>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddScoped<ISolarPowerPlantService, SolarPowerPlantService>();
    builder.Services.AddScoped<ISeedService, SeedService>();

    var app = builder.Build();

    app.UseMiddleware<ErrorHandlerMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}