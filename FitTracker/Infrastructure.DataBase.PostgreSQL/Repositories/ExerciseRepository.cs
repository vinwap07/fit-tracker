using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ExerciseRepository: IExerciseRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Exercise> _dbSet;

    public ExerciseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Exercise>();
    }
    
    public Task<Exercise?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveAsync(Exercise aggregateRoot, CancellationToken ct)
    {
        var exercise = await _dbSet.FirstOrDefaultAsync(x => x.Id == aggregateRoot.Id, ct);
        
        if (exercise == null)
        {
            _dbSet.Add(aggregateRoot);
        }
        else
        {
            _dbSet.Update(aggregateRoot);
        }
    }

    public async Task DeleteAsync(Exercise aggregateRoot, CancellationToken ct)
    {
        await _dbSet.Where(x => x.Id == aggregateRoot.Id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<List<Exercise>> GetAllAsync(CancellationToken ct)
    {
        return await _dbSet.AsNoTracking().ToListAsync(ct);
    }

    public async Task<List<Exercise>> GetByIdsAsync(List<Guid> ids, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(ct);
    }

    public Task<Exercise?> GetByNameAsync(string name, CancellationToken ct)
    {
        return _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, ct);
    }
}