using Application.DTOs.ExerciseProposal;
using Application.Workouts.GetWorkoutInformation;
using MediatR;

namespace Application.Exercises.Proposals;

public record GetOpenProposalsQuery : IRequest<List<ExerciseProposalListItemDto>>;

public class GetOpenProposalsQueryHandler(
    IExerciseProposalQueries exerciseProposalQueries
) : IRequestHandler<GetOpenProposalsQuery, List<ExerciseProposalListItemDto>>
{
    public async Task<List<ExerciseProposalListItemDto>> Handle(GetOpenProposalsQuery request, CancellationToken ct)
    {
        return await exerciseProposalQueries.GetOpenExerciseProposalsAsync(ct); 
    }
}
