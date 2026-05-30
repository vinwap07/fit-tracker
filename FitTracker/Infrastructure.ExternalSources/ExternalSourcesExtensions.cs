using Application.Abstractions;
using Infrastructure.ExternalSources.Geo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalSources;

public static class ExternalSourcesExtensions
{
    public static IServiceCollection AddApiIntegration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var section = configuration.GetSection(WeatherApiOptions.SectionName);
        services.Configure<WeatherApiOptions>(section);
        
        var options = section.Get<WeatherApiOptions>() ??
                      throw new ArgumentNullException(nameof(WeatherApiOptions.SectionName));
        
        services.AddHttpClient<IWeatherService, WeatherService>(client =>
        {
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        services.AddHttpClient<IGeoLocationService, GeoLocationService>(client =>
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "FitTracker-WebApi");
        });
        
        return services;
    }
}