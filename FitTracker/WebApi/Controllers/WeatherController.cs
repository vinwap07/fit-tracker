using Application.Abstractions;
using Application.DTOs.UserHealth;
using Application.PersonalHealth;
using Application.Weather;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Presentation.Controllers;

public class WeatherController: BaseApiController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<WeatherRecommendation>> GetPersonalHealthAccount(CancellationToken ct)
    {
        var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
        var ip = remoteIpAddress?.ToString() ?? string.Empty;
        var query = new GetWeatherRecommendationQuery(ip);
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }
}