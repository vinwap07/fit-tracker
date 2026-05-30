using Application.DTOs.Auth;

namespace Application.Abstractions.Queries;

public interface IAccountQueries
{
    Task<AccountInfoDto?> GetByIdAsync(Guid accountId, CancellationToken ct);
}