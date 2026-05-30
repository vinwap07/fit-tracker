using System.Text.Json.Serialization;

namespace Infrastructure.ExternalSources;

public record OpenWeatherResponse(
    [property: JsonPropertyName("main")] MainInfo Main,
    [property: JsonPropertyName("weather")] WeatherDescription[] Weather,
    [property: JsonPropertyName("wind")] WindInfo? Wind,
    [property: JsonPropertyName("name")] string CityName);

public record MainInfo(
    [property: JsonPropertyName("temp")] decimal Temp,
    [property: JsonPropertyName("feels_like")] decimal FeelsLike,
    [property: JsonPropertyName("humidity")] int Humidity);

public record WeatherDescription(
    [property: JsonPropertyName("main")] string Main, 
    [property: JsonPropertyName("description")] string Description, 
    [property: JsonPropertyName("icon")] string Icon);

public record WindInfo(
    [property: JsonPropertyName("speed")] decimal Speed);