using Application.DTOs.ExerciseProposal;
using Application.Workouts.GetWorkoutInformation;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Queries;

public class ExerciseProposalQueries(
    ApplicationDbContext context,
    IMapper mapper
    ): IExerciseProposalQueries
{
    private readonly DbSet<ExerciseProposal> _dbSet = context.ExerciseProposals;
    
    public async Task<List<ExerciseProposalListItemDto>> GetOpenExerciseProposalsAsync(CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.Status == ProposalStatus.Pending)
            .ProjectTo<ExerciseProposalListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<List<ExerciseProposalListItemDto>> GetUsersExerciseProposalsAsync(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.AuthorId == userId)
            .ProjectTo<ExerciseProposalListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<ExerciseProposalDetails?> GetByIdAsync(Guid proposalId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.Id == proposalId)
            .ProjectTo<ExerciseProposalDetails>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }
}