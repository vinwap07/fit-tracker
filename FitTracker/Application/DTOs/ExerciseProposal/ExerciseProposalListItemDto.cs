using Domain.Aggregates;

namespace Application.DTOs.ExerciseProposal;

public class ExerciseProposalListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public ProposalStatus Status { get; init; }
}