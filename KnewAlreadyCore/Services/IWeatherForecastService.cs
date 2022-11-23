using KnewAlreadyWebApp.Dtos;

namespace KnewAlreadyWebApp.Data;

public interface IWeatherForecastService
{
    Task<WeatherForecastDto[]> GetForecasts();
}