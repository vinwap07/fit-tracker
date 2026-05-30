using Domain.Aggregates;

namespace Domain.Repositories;

public interface IExerciseProposalRepository: IRepository<ExerciseProposal>
{
    Task<List<ExerciseProposal>?> GetPendingProposalsAsync(CancellationToken ct);
    Task<List<ExerciseProposal>?> GetApprovedProposalsAsync(CancellationToken ct);
    Task<List<ExerciseProposal>?> GetUsersProposalsAsync(Guid userId, CancellationToken ct);
}