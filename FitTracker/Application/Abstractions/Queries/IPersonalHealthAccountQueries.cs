using Application.DTOs.UserHealth;

namespace Application.Abstractions.Queries;

public interface IPersonalHealthAccountQueries
{
    Task<PersonalHealthAccountDto?> GetByIdAsync(Guid accountId, CancellationToken ct);
    Task<List<WeightHistoryListItemDto>> GetUsersWeightHistoryAsync(Guid userId, CancellationToken ct);
}