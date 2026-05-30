using Domain.Aggregates;

namespace Domain.Repositories;

public interface IWeightHistoryPointRepository: IRepository<WeightHistoryPoint>
{
    Task<List<WeightHistoryPoint>?> GetByUserId(Guid userId, CancellationToken ct);
    Task<List<WeightHistoryPoint>?> GetByUserIdAndDateRange(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken ct);
}