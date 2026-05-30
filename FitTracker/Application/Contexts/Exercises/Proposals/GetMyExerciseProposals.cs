using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;
using Application.DTOs.ExerciseProposal;
using Application.Workouts.GetWorkoutInformation;
using MediatR;

namespace Application.Exercises.Proposals;

public record GetMyExerciseProposalsQuery: IRequest<List<ExerciseProposalListItemDto>>; 

public class GetMyExerciseProposalsQueryHandler(
    IExerciseProposalQueries exerciseProposalQueries,
    ICurrentUserAccessor currentUserAccessor
) : IRequestHandler<GetMyExerciseProposalsQuery, List<ExerciseProposalListItemDto>>
{
    private readonly CurrentUser _currentUser = currentUserAccessor.GetCurrentUser();
    
    public async Task<List<ExerciseProposalListItemDto>> Handle(GetMyExerciseProposalsQuery request, CancellationToken ct)
    {
        return await exerciseProposalQueries.GetUsersExerciseProposalsAsync(_currentUser.Id, ct); 
    }
}