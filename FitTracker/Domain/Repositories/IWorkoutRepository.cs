using Domain.Aggregates;

namespace Domain.Repositories;

public interface IWorkoutRepository: IRepository<Workout>
{
    Task<List<Workout>>? GetByUserAsync(Guid userId, CancellationToken ct);
    Task<List<Workout>>? GetByUserAndDateAsync(Guid userId, DateTime date, CancellationToken ct);
}