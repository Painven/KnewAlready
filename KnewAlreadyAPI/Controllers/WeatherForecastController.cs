using KnewAlreadyWebApp.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Route("api/forecasts")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecastDto> GetAll()
    {
        var data = Enumerable.Range(0, 100)
            .Select(i => new WeatherForecastDto()
            {
                Date = DateTime.Now.AddDays(-i),
                Summary = i.ToString(),
                TemperatureC = 100 - i
            })
            .ToArray();

        return data;
    }
}
