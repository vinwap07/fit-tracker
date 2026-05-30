using Application.Abstractions.Queries;
using Application.DTOs.Workout;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Queries;

public class WorkoutQueries(
    ApplicationDbContext context,
    IMapper mapper
    ): IWorkoutQueries
{
    private readonly DbSet<Workout> _dbSet = context.Workouts;
    
    public async Task<List<WorkoutCaloriesChartPointDto>> GetWorkoutCaloriesChartPointsAsync(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .ProjectTo<WorkoutCaloriesChartPointDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<List<WorkoutChartPointDto>> GetWorkoutChartPointsAsync(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .ProjectTo<WorkoutChartPointDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<List<WorkoutListItemDto>> GetUsersWorkoutsAsync(Guid userId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .ProjectTo<WorkoutListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }

    public async Task<WorkoutDetailsDto?> GetByIdAsync(Guid workoutId, CancellationToken ct)
    {
        return await _dbSet.AsNoTracking()
            .Where(w => w.Id == workoutId)
            .ProjectTo<WorkoutDetailsDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct);
    }
}