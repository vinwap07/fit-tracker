using Application.DTOs.ExerciseProposal;
using Application.DTOs.Workout;
using Application.Workouts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Presentation.Controllers;

public class WorkoutController: BaseApiController
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateWorkout(
        CreateWorkoutCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteWorkout(
        [FromRoute] Guid id, DeleteWorkoutCommand command, CancellationToken ct)
    {
        var commandWithId = new DeleteWorkoutCommand(id);
        await Mediator.Send(commandWithId, ct);
        return Ok();
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<List<WorkoutListItemDto>>> GetMyWorkouts(
        [FromQuery] GetUsersWorkoutsQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("my/calories-chart")]
    public async Task<ActionResult<List<WorkoutCaloriesChartPointDto>>> GetMyWorkoutCaloriesChart(
        [FromQuery] GetWorkoutCaloriesChartQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("my/date-chart")]
    public async Task<ActionResult<List<WorkoutChartPointDto>>> GetMyWorkoutChart(
        [FromQuery] GetWorkoutChartQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("my/{id:guid}")]
    public async Task<ActionResult<WorkoutDetailsDto>> GetMyWorkoutChart(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetWorkoutDetailsQuery(id);
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateWorkout(
        [FromRoute] Guid id, UpdateWorkoutCommand command, CancellationToken ct)
    {
        var commandWithId = command with { WorkoutId = id };
        var result = await Mediator.Send(commandWithId, ct);
        return Ok(result);
    }
    
}