using Domain.Aggregates;

namespace Domain.Repositories;

public interface IUserAccountRepository: IRepository<UserAccount> 
{
    Task<UserAccount?> GetByEmailAsync(string email, CancellationToken ct);
}