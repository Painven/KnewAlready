using KnewAlreadyCore.Dtos;
using KnewAlreadyWebApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Route("api/answer-share")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly Dictionary<string, string> activeApiKeys = new ()
    {
        ["661b4931-29fa-4ed3-b40e-c8377132170d"] = "painven",
        ["651b4931-29fa-4ed3-b40e-c8377132170d"] = "test user",
    };

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<AnswerShareResponseDto> GetAll()
    {
        
    }

    [HttpPost]
    public AnswerShareResponseDto Send([FromBody]AnswerShareRequestDto data)
    {
        
        AnswerShareResponseDto response = new AnswerShareResponseDto();


        _logger.LogInformation($"Запрос получен: {JsonSerializer.Serialize(data)}");
        _logger.LogInformation($"Ответ отправлен: {JsonSerializer.Serialize(response)}");
    }
}
