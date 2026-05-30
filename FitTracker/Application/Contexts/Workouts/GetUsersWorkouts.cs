using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.Auth;
using Application.DTOs.Workout;
using MediatR;

namespace Application.Workouts;

public record GetUsersWorkoutsQuery: IRequest<List<WorkoutListItemDto>>; 

public class GetUsersWorkoutsQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkoutQueries workoutQueries
) : IRequestHandler<GetUsersWorkoutsQuery, List<WorkoutListItemDto>>
{
    public async Task<List<WorkoutListItemDto>> Handle(GetUsersWorkoutsQuery request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        return await workoutQueries.GetUsersWorkoutsAsync(currentUser.Id, ct);
    }
}