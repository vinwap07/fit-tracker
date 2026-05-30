namespace Application.Abstractions;

public interface IGeoLocationService
{
    Task<string> GetCityByIpAsync(string ip);
}