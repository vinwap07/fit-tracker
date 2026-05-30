using Application.Abstractions;
using Infrastructure.Data.Configurations;
using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<UserAccount> UserAccounts => Set<UserAccount>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<ExerciseProposal> ExerciseProposals => Set<ExerciseProposal>();
    public DbSet<PersonalHealthAccount> PersonalHealthAccounts => Set<PersonalHealthAccount>();
    public DbSet<WeightHistoryPoint> WeightHistoryPoint => Set<WeightHistoryPoint>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}