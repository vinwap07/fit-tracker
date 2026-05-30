using Application.DTOs.ExerciseProposal;

namespace Application.Workouts.GetWorkoutInformation;

public interface IExerciseProposalQueries
{
    Task<List<ExerciseProposalListItemDto>> GetOpenExerciseProposalsAsync(CancellationToken ct);
    Task<List<ExerciseProposalListItemDto>> GetUsersExerciseProposalsAsync(Guid userId, CancellationToken ct);
    Task<ExerciseProposalDetails?> GetByIdAsync(Guid proposalId, CancellationToken ct);
}