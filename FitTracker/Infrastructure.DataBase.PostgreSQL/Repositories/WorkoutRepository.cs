using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class WorkoutRepository: IWorkoutRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Workout> _dbSet;

    public WorkoutRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Workout>();
    }
    
    public async Task<Workout?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(w => w.Id == id, ct);
    }

    public async Task SaveAsync(Workout aggregateRoot, CancellationToken ct)
    {
        var workout = await _dbSet.FirstOrDefaultAsync(w => w.Id == aggregateRoot.Id, ct);

        if (workout == null)
        {
            _dbSet.Add(aggregateRoot);
        }
        else
        {
            _dbSet.Update(aggregateRoot);
        }
    }

    public async Task DeleteAsync(Workout aggregateRoot, CancellationToken ct)
    {
        await _dbSet.Where(x => x.Id == aggregateRoot.Id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<List<Workout>>? GetByUserAsync(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task<List<Workout>>? GetByUserAndDateAsync(Guid userId, DateTime date, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .Where(w => w.Date == date)
            .ToListAsync(ct);
    }
}