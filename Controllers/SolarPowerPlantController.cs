using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerPlant.Models;
using PowerPlant.Services;

namespace PowerPlant.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class SolarPowerPlantController : ControllerBase
    {
        private readonly ISolarPowerPlantService _solarPowerPlantService;
        public SolarPowerPlantController(ISolarPowerPlantService solarPowerPlantService)
        {
            _solarPowerPlantService = solarPowerPlantService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] SolarPowerPlantModel model)
        {
            if (ModelState.IsValid)
            {
                await _solarPowerPlantService.CreateOrUpdate(model);
                return Ok(new { message = "Successful" });
            }

            var error = string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest("Create or update failed. " + error);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _solarPowerPlantService.GetById(id);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _solarPowerPlantService.GetAll();
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _solarPowerPlantService.Delete(id);
            return Ok(new { message = "Successful" });
        }

        [HttpGet]
        public async Task<IActionResult> GetTimeSeriesByFilters([FromQuery] GetByFilterModel model)
        {
            if (ModelState.IsValid)
            {
                var res = await _solarPowerPlantService.GetTimeSeriesByFilters(model);
                return Ok(res);
            }

            var error = string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest("Operation failed. " + error);
        }

    }
}
