using Domain.Aggregates;

namespace Domain.Repositories;

public interface IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
{
    Task<TAggregateRoot?> GetByIdAsync(Guid id, CancellationToken ct);
    Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken ct);
    Task DeleteAsync(TAggregateRoot aggregateRoot, CancellationToken ct);
}