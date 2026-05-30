using System.Net.Http.Json;
using Application.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalSources;

public class WeatherService(
    HttpClient httpClient,
    IOptions<WeatherApiOptions> options,
    ILogger<WeatherService> logger) : IWeatherService
{
    public async Task<WeatherRecommendation> GetRecommendationAsync(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            city = "Moscow";

        var apiKey = options.Value.ApiKey;
        try
        {
            var encodedCity = Uri.EscapeDataString(city.Trim());
            var response = await httpClient.GetFromJsonAsync<OpenWeatherResponse>(
                $"weather?q={encodedCity}&appid={apiKey}&units=metric&lang=ru");

            if (response?.Main is null || response.Weather is null || response.Weather.Length == 0)
            {
                logger.LogWarning("OpenWeather returned empty payload for city {City}", city);
                return FallbackRecommendation();
            }

            var temp = response.Main.Temp;
            var isRaining = response.Weather.Any(w =>
                !string.IsNullOrEmpty(w.Main) && w.Main.Contains("Rain", StringComparison.OrdinalIgnoreCase));
            var isSnowing = response.Weather.Any(w =>
                !string.IsNullOrEmpty(w.Main) && w.Main.Contains("Snow", StringComparison.OrdinalIgnoreCase));
            var wind = response.Wind?.Speed ?? 0;

            var isGood = temp > 5 && temp < 25 && wind < 10 && !isRaining && !isSnowing;

            var message = isGood
                ? "Погода хорошая для пробежки."
                : "Лучше потренироваться дома или в зале.";

            return new WeatherRecommendation(
                temp,
                response.Weather[0].Description,
                isGood,
                message);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Weather API failed for city {City}", city);
            return FallbackRecommendation();
        }
    }

    private static WeatherRecommendation FallbackRecommendation()
    {
        return new WeatherRecommendation(
            15,
            "Нет данных с сервиса погоды.",
            false,
            "Не удалось получить актуальный прогноз. Проверьте ключ API и интернет-соединение.");
    }
}