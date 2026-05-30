using Application.Abstractions.Queries;
using Application.DTOs.Exercise;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Queries;

public class ExerciseQueries(
    ApplicationDbContext context,
    IMapper mapper
    ): IExerciseQueries
{
    private readonly DbSet<Exercise> _dbSet = context.Exercises;
    
    public async Task<List<ExerciseListItem>> GetExercisesAsync(CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .ProjectTo<ExerciseListItem>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<List<ExerciseListItem>> GetExercisesByNameAsync(string name, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(e => e.Name.StartsWith(name))
            .ProjectTo<ExerciseListItem>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<ExerciseDetail?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<ExerciseDetail>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }
}