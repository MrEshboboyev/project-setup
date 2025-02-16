using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    [
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        logger.LogInformation("Fetching weather forecast data.");
        using var rng = RandomNumberGenerator.Create();
        return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = GetSecureRandomNumber(rng, -20, 55),
            Summary = Summaries[GetSecureRandomNumber(rng, 0, Summaries.Length)]
        })];
    }

    private static int GetSecureRandomNumber(RandomNumberGenerator rng, int minValue, int maxValue)
    {
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        int value = BitConverter.ToInt32(randomNumber, 0) & int.MaxValue;
        return (int)(minValue + value % (uint)(maxValue - minValue));
    }
}
