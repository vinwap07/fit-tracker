using Domain.Aggregates;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class PersonalHealthAccountRepository: IPersonalHealthAccountRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<PersonalHealthAccount> _dbSet;

    public PersonalHealthAccountRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<PersonalHealthAccount>();
    }
    
    public async Task<PersonalHealthAccount?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task SaveAsync(PersonalHealthAccount aggregateRoot, CancellationToken ct)
    {
        var data = await _dbSet.FirstOrDefaultAsync(x => x.Id == aggregateRoot.Id, ct);
        
        if (data == null)
        {
            _dbSet.Add(aggregateRoot);
        }
        else
        {
            _dbSet.Update(aggregateRoot);
        }
    }

    public async Task DeleteAsync(PersonalHealthAccount aggregateRoot, CancellationToken ct)
    {
        await _dbSet.Where(x => x.Id == aggregateRoot.Id)
            .ExecuteDeleteAsync(ct);
    }
}