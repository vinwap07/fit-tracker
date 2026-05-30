namespace Application.Abstractions;

public interface IWeatherService
{
    Task<WeatherRecommendation> GetRecommendationAsync(string city);
}

public record WeatherRecommendation(
    decimal Temperature, 
    string Description, 
    bool IsRunningRecommended, 
    string MotivationMessage);