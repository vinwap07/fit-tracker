using Domain.Aggregates;

namespace Application.DTOs.ExerciseProposal;

public class ExerciseProposalDetailsDto
{
    public Guid Id { get; init; }
    public Guid AuthorId { get; init; }
    public ProposalStatus Status { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PerformanceVideoUrl { get; init; } = string.Empty;
    public string EmgVideoUrl { get; init; } = string.Empty;
    public string PhotoUrl { get; init; } = string.Empty;
}