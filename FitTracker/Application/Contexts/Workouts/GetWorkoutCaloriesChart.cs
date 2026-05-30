using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.Auth;
using Application.DTOs.Workout;
using MediatR;

namespace Application.Workouts;

public record GetWorkoutCaloriesChartQuery : IRequest<List<WorkoutCaloriesChartPointDto>>;

public class GetWorkoutCaloriesChartQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IWorkoutQueries workoutQueries)
    : IRequestHandler<GetWorkoutCaloriesChartQuery, List<WorkoutCaloriesChartPointDto>>
{
    private readonly CurrentUser _currentUser = currentUserAccessor.GetCurrentUser();

    public async Task<List<WorkoutCaloriesChartPointDto>> Handle(GetWorkoutCaloriesChartQuery request, CancellationToken ct)
    {
        return await workoutQueries.GetWorkoutCaloriesChartPointsAsync(_currentUser.Id, ct);
    }
}