using Domain.Aggregates;

namespace Domain.Repositories;

public interface IExerciseRepository: IRepository<Exercise>
{
    Task<List<Exercise>> GetAllAsync(CancellationToken ct);
    Task<List<Exercise>> GetByIdsAsync(List<Guid> ids, CancellationToken ct);
    Task<Exercise?> GetByNameAsync(string name, CancellationToken ct);
}