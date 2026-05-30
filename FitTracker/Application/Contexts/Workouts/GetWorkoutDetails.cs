using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.Auth;
using Application.DTOs.Workout;
using Application.Exceptions;
using MediatR;

namespace Application.Workouts;

public record GetWorkoutDetailsQuery(Guid WorkoutId) : IRequest<WorkoutDetailsDto>;

public class GetWorkoutDetailsQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkoutQueries workoutQueries)
    : IRequestHandler<GetWorkoutDetailsQuery, WorkoutDetailsDto>
{
    public async Task<WorkoutDetailsDto?> Handle(GetWorkoutDetailsQuery request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        
        var workoutDetailsDto = await workoutQueries.GetByIdAsync(request.WorkoutId, ct);
        if (workoutDetailsDto.UserId != currentUser.Id)
        {
            throw new ForbiddenAccessException();
        }
        
        return workoutDetailsDto;
    }
}