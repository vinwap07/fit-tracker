using Application.DTOs.Exercise;
using Application.Exercises;

namespace Application.Abstractions.Queries;

public interface IExerciseQueries
{
    Task<List<ExerciseListItem>> GetExercisesAsync(CancellationToken ct);
    Task<List<ExerciseListItem>> GetExercisesByNameAsync(string name, CancellationToken ct);
    Task<ExerciseDetail?> GetByIdAsync(Guid id, CancellationToken ct);
}