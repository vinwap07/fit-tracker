using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class WeightHistoryPointRepository: IWeightHistoryPointRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<WeightHistoryPoint> _dbSet;

    public WeightHistoryPointRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<WeightHistoryPoint>();
    }
    
    public async Task<WeightHistoryPoint?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveAsync(WeightHistoryPoint aggregateRoot, CancellationToken ct)
    {
        var weightHistory = await _dbSet.FirstOrDefaultAsync(x => x.Id == aggregateRoot.Id, ct);
        
        if (weightHistory == null)
        {
            _dbSet.Add(aggregateRoot);
        }
        else
        {
            _dbSet.Update(aggregateRoot);
        }
    }

    public async Task DeleteAsync(WeightHistoryPoint aggregateRoot, CancellationToken ct)
    {
        await _dbSet.Where(x => x.Id == aggregateRoot.Id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<List<WeightHistoryPoint>?> GetByUserId(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<List<WeightHistoryPoint>?> GetByUserIdAndDateRange(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Where(x => x.Date >= startDate && x.Date <= endDate)
            .ToListAsync(ct);
    }
}