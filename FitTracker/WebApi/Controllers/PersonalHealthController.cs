using Application.DTOs.Exercise;
using Application.DTOs.UserHealth;
using Application.Exercises;
using Application.PersonalHealth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Presentation.Controllers;

public class PersonalHealthController : BaseApiController
{
    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<PersonalHealthAccountDto>> GetPersonalHealthAccount(
        [FromQuery] GetPersonalHealthAccountQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("my/weight-history")]
    public async Task<ActionResult<List<WeightHistoryListItemDto>>> GetWeightHistory(
        [FromQuery] GetMyWeightHistoryQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePersonalHealthAccount(
        CreatePersonalHealthAccountCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("my")]
    public async Task<ActionResult<Guid>> UpdatePersonalHealthAccount(
        UpdatePersonalHealthAccountCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return Ok(result);
    }
}