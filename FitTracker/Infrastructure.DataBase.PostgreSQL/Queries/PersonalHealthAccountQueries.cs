using Application.Abstractions.Queries;
using Application.DTOs.UserHealth;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Queries;

public class PersonalHealthAccountQueries(
    ApplicationDbContext context,
    IMapper mapper
    ): IPersonalHealthAccountQueries
{
    private readonly DbSet<PersonalHealthAccount> _personalHealthAccounts = context.PersonalHealthAccounts;
    private readonly DbSet<WeightHistoryPoint> _weightHistories = context.WeightHistoryPoint;
    
    public async Task<PersonalHealthAccountDto?> GetByIdAsync(Guid accountId, CancellationToken ct)
    {
        return await _personalHealthAccounts.AsNoTracking()
            .Where(x => x.Id == accountId)
            .ProjectTo<PersonalHealthAccountDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<List<WeightHistoryListItemDto>> GetUsersWeightHistoryAsync(Guid userId, CancellationToken ct)
    {
        return await _weightHistories.AsNoTracking()
            .Where(x => x.UserId == userId)
            .ProjectTo<WeightHistoryListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }
}