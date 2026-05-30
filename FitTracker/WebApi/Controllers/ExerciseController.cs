using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Exercise;
using Application.Exercises;
using Microsoft.AspNetCore.Authorization;
using WebApi.Controllers;

namespace Presentation.Controllers;


public class ExerciseCatalogController() : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<ExerciseListItemDto>>> GetExercisesAsync(
        [FromQuery] GetExercisesQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<ActionResult<List<ExerciseListItemDto>>> SearchExercisesAsync(
        [FromQuery] GetExercisesByNameQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseListItemDto>> GetExerciseAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetExerciseDetailsQuery(id);    
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }
}