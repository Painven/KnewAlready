using KnewAlreadyCore.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KnewAlreadyAPI.Controllers;

[ApiController]
[Route("api/answer-share")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly Dictionary<string, string> activeApiKeys;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<AnswerShareResponseDto> GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public AnswerShareResponseDto Send([FromBody]AnswerShareRequestDto data)
    {
        throw new NotImplementedException();
        AnswerShareResponseDto response = new AnswerShareResponseDto();


        _logger.LogInformation($"Запрос получен: {JsonSerializer.Serialize(data)}");
        _logger.LogInformation($"Ответ отправлен: {JsonSerializer.Serialize(response)}");
    }
}
