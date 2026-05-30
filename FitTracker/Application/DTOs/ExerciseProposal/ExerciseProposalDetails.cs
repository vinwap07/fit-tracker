using Domain.Aggregates;

namespace Application.DTOs.ExerciseProposal;

public class ExerciseProposalDetails
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid AuthorId { get; init; }
    public ProposalStatus Status { get; init; }
    public string PerformanceVideoKey { get; init; } = string.Empty;
    public string EmgVideoKey { get; init; } = string.Empty;
    public string PhotoKey { get; init; } = string.Empty;
    public string Bucket { get; init; } = string.Empty;
    public string PerformanceVideoUrl { get; set; } = string.Empty;
    public string EmgVideoUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}