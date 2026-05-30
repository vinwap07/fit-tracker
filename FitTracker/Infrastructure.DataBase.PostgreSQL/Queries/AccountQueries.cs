using Application.Abstractions.Queries;
using Application.DTOs.Auth;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Queries;

public class AccountQueries(
    ApplicationDbContext context,
    IMapper mapper
    ) : IAccountQueries
{
    private readonly DbSet<UserAccount> _dbSet = context.UserAccounts;
    public async Task<AccountInfoDto?> GetByIdAsync(Guid accountId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(account => account.Id == accountId)
            .ProjectTo<AccountInfoDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }
}