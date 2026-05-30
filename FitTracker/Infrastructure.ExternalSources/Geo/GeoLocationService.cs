using System.Net;
using System.Net.Http.Json;
using Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ExternalSources.Geo;

public class GeoLocationService(HttpClient httpClient, ILogger<GeoLocationService> logger) : IGeoLocationService
{
    private const string DefaultCity = "Moscow";

    public async Task<string> GetCityByIpAsync(string ip)
    {
        if (!TryGetPublicIpv4(ip, out ip))
            return DefaultCity;

        try
        {
            var escaped = Uri.EscapeDataString(ip);
            using var resp = await httpClient.GetAsync($"http://ip-api.com/json/{escaped}");
            if (!resp.IsSuccessStatusCode)
                return DefaultCity;

            var response = await resp.Content.ReadFromJsonAsync<GeoResponse>();
            if (response?.Status == "success" && !string.IsNullOrWhiteSpace(response.City))
                return response.City;
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Geo lookup failed for IP {Ip}", ip);
        }

        return DefaultCity;
    }

    /// <summary>
    /// ip-api rejects loopback/private; browser / dev proxies often report ::1.
    /// </summary>
    private static bool TryGetPublicIpv4(string? rawIp, out string ip)
    {
        ip = string.Empty;
        if (string.IsNullOrWhiteSpace(rawIp))
            return false;

        var trimmed = rawIp.Trim();

        // Forwarded IPv4 (e.g. by reverse proxy): "151.x.x.x" or "::ffff:151.x.x.x"
        var scope = trimmed.IndexOf('%');
        if (scope >= 0)
            trimmed = trimmed[..scope];

        if (!IPAddress.TryParse(trimmed, out var address))
            return false;

        if (IPAddress.IsLoopback(address))
            return false;

        // Map IPv6-mapped IPv4 to dotted quad for ip-api
        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6 &&
            trimmed.StartsWith("::ffff:", StringComparison.OrdinalIgnoreCase))
        {
            var v4Part = trimmed["::ffff:".Length..];
            if (IPAddress.TryParse(v4Part, out var v4))
            {
                ip = v4.ToString();
                return !IPAddress.IsLoopback(v4);
            }
        }

        if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            ip = trimmed;
            return !IPAddress.IsLoopback(address);
        }

        return false;
    }
}

public record GeoResponse(string Status, string City);