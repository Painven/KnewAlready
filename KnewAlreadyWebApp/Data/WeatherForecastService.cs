using KnewAlreadyWebApp.Dtos;
using System.Net.Http.Json;

namespace KnewAlreadyWebApp.Data;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IHttpClientFactory httpClientFactory;

    public WeatherForecastService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<WeatherForecastDto[]> GetForecasts()
    {
        var client = httpClientFactory.CreateClient("KnewAlreadyAPI");

        var data = await client.GetFromJsonAsync<WeatherForecastDto[]>("api/forecasts");

        return data;
    }
}
