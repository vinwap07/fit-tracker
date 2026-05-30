using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class UserAccountRepository: IUserAccountRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<UserAccount> _dbSet;

    public UserAccountRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<UserAccount>();
    }
    
    public async Task<UserAccount?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveAsync(UserAccount aggregateRoot, CancellationToken ct)
    {
        var account = await _dbSet.FirstOrDefaultAsync(x => x.Id == aggregateRoot.Id, ct);

        if (account == null)
        {
            _dbSet.Add(aggregateRoot);
        }
        else
        {
            _dbSet.Update(aggregateRoot);
        }
    }

    public async Task DeleteAsync(UserAccount aggregateRoot, CancellationToken ct)
    {
        await _dbSet.Where(x => x.Id == aggregateRoot.Id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task<UserAccount?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Email == email, ct);
    }
}