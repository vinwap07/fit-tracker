using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ExerciseProposalRepository: IExerciseProposalRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<ExerciseProposal> _dbSet;

    public ExerciseProposalRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<ExerciseProposal>();
    }
    public Task<ExerciseProposal?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _dbSet
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveAsync(ExerciseProposal aggregateRoot, CancellationToken ct)
    {
        var request = await _dbSet.FirstOrDefaultAsync(x => x.Id == aggregateRoot.Id, ct);

        if (request == null)
        {
            _dbSet.Add(aggregateRoot);
        }
        else
        {
            _dbSet.Update(aggregateRoot);
        }
    }

    public async Task DeleteAsync(ExerciseProposal aggregateRoot, CancellationToken ct)
    {
        await _dbSet.Where(x => x.Id == aggregateRoot.Id)
            .ExecuteDeleteAsync(ct);
    }

    public Task<List<ExerciseProposal>> GetPendingProposalsAsync(CancellationToken ct)
    {
        return _dbSet.AsNoTracking()
            .Where(x => x.Status == ProposalStatus.Pending)
            .ToListAsync(ct);
    }

    public async Task<List<ExerciseProposal>> GetApprovedProposalsAsync(CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.Status == ProposalStatus.Approved)
            .ToListAsync(ct);
    }

    public async Task<List<ExerciseProposal>> GetUsersProposalsAsync(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.AuthorId == userId)
            .ToListAsync(ct);
    }
}