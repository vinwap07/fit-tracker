using Application.Abstractions;
using MediatR;

namespace Application.Weather;

public record GetWeatherRecommendationQuery(string Ip) : IRequest<WeatherRecommendation>;

public class GetWeatherRecommendationQueryHandler(
    IGeoLocationService geoLocationService,
    IWeatherService weatherService
    ) : IRequestHandler<GetWeatherRecommendationQuery, WeatherRecommendation>
{
    public async Task<WeatherRecommendation> Handle(GetWeatherRecommendationQuery request, CancellationToken cancellationToken)
    {
        var city = await geoLocationService.GetCityByIpAsync(request.Ip);
        return await weatherService.GetRecommendationAsync(city);
    }
}