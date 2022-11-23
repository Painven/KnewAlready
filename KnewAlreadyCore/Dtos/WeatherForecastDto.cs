namespace KnewAlreadyWebApp.Dtos;

public record WeatherForecastDto
{
    public DateTime Date { get; init; }
    public int TemperatureC { get; init; }
    public string? Summary { get; init; }
}
