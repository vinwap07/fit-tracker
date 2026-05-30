using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.Auth;
using Application.DTOs.Workout;
using MediatR;

namespace Application.Workouts;

public record GetWorkoutChartQuery : IRequest<List<WorkoutChartPointDto>>;

public class GetWorkoutChartQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkoutQueries workoutQueries)
    : IRequestHandler<GetWorkoutChartQuery, List<WorkoutChartPointDto>>
{
    private readonly CurrentUser _currentUser = currentUserAccessor.GetCurrentUser();
    
    public async Task<List<WorkoutChartPointDto>> Handle(GetWorkoutChartQuery request, CancellationToken ct)
    {
        return await workoutQueries.GetWorkoutChartPointsAsync(_currentUser.Id, ct);
    }
}