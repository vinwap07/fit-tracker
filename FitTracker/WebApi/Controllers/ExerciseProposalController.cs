using Microsoft.AspNetCore.Mvc;
using Application.DTOs.ExerciseProposal;
using Application.Exercises;
using Application.Exercises.Proposals;
using Microsoft.AspNetCore.Authorization;
using WebApi.Controllers;

namespace Presentation.Controllers;

public class ProposeExerciseFormModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Met { get; set; }
    public IFormFile Photo { get; set; } = null!;
    public IFormFile PerformanceVideo { get; set; } = null!;
    public IFormFile EmgVideo { get; set; } = null!;
}

public class ExerciseProposalController() : BaseApiController
{
    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<Guid>> ProposeExerciseAsync(
        [FromForm] ProposeExerciseFormModel model,
        CancellationToken ct)
    {
        if (model.PerformanceVideo is null || model.PerformanceVideo.Length == 0)
            return BadRequest(new { message = "performanceVideo is required" });
        if (model.EmgVideo is null || model.EmgVideo.Length == 0)
            return BadRequest(new { message = "emgVideo is required" });
        if (model.Photo is null || model.Photo.Length == 0)
            return BadRequest(new { message = "photo is required" });

        await using var perfStream = model.PerformanceVideo.OpenReadStream();
        await using var emgStream = model.EmgVideo.OpenReadStream();
        await using var photoStream = model.Photo.OpenReadStream();

        var command = new ProposeExerciseCommand(
            model.Name,
            model.Description,
            model.Met,
            new FileParameter(perfStream, model.PerformanceVideo.FileName,
                model.PerformanceVideo.ContentType ?? "application/octet-stream"),
            new FileParameter(emgStream, model.EmgVideo.FileName,
                model.EmgVideo.ContentType ?? "application/octet-stream"),
            new FileParameter(photoStream, model.Photo.FileName,
                model.Photo.ContentType ?? "application/octet-stream"));

        var proposalId = await Mediator.Send(command, ct);
        return Ok(proposalId);
    }

    [Authorize(Roles = "Moderator")]
    [HttpGet]
    public async Task<ActionResult<List<ExerciseProposalListItemDto>>> GetOpenExerciseProposalsAsync(
        [FromQuery] GetOpenProposalsQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ExerciseProposalDetailsDto>> GetExerciseProposalAsync(
        [FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetExerciseProposalDetailsQuery(id);
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }

    [Authorize(Roles = "Moderator")]
    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult> ApproveExerciseProposalAsync(
        [FromRoute] Guid id, ApproveExerciseProposalCommand command, CancellationToken ct)
    {
        var commandWithId = command with { ProposalId = id };
        await Mediator.Send(commandWithId, ct);
        return Ok();
    }

    [Authorize(Roles = "Moderator")]
    [HttpPost("{id:guid}/reject")]
    public async Task<ActionResult> RejectExerciseProposalAsync(
        [FromRoute] Guid id, RejectExerciseProposalCommand command, CancellationToken ct)
    {
        var commandWithId = command with { ProposalId = id };
        await Mediator.Send(commandWithId, ct);
        return Ok();
    }

    [Authorize(Roles = "Moderator")]
    [HttpPost("{id:guid}/send-to-revision")]
    public async Task<ActionResult> SendToRevisionExerciseProposalAsync(
        [FromRoute] Guid id, SendExerciseProposalToRevisionCommand command, CancellationToken ct)
    {
        var commandWithId = command with { ProposalId = id };
        await Mediator.Send(commandWithId, ct);
        return Ok();
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<ExerciseProposalDetailsDto>> GetMyExerciseProposalsAsync(
        [FromQuery] GetMyExerciseProposalsQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }
}
