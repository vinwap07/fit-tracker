using Application.DTOs.Workout;

namespace Application.Abstractions.Queries;

public interface IWorkoutQueries
{
    Task<List<WorkoutCaloriesChartPointDto>> GetWorkoutCaloriesChartPointsAsync(Guid userId, CancellationToken ct);
    Task<List<WorkoutChartPointDto>> GetWorkoutChartPointsAsync(Guid userId, CancellationToken ct);
    Task<List<WorkoutListItemDto>> GetUsersWorkoutsAsync(Guid userId, CancellationToken ct);
    Task<WorkoutDetailsDto?> GetByIdAsync(Guid workoutId, CancellationToken ct);
}