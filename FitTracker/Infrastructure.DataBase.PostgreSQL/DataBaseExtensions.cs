using Application.Abstractions;
using Application.Abstractions.Queries;
using Application.Workouts.GetWorkoutInformation;
using Domain.Repositories;
using Infrastructure.Data.Queries;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class DataBaseExtensions
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IExerciseProposalRepository, ExerciseProposalRepository>();
        services.AddScoped<IPersonalHealthAccountRepository, PersonalHealthAccountRepository>();
        services.AddScoped<IUserAccountRepository, UserAccountRepository>();
        services.AddScoped<IWeightHistoryPointRepository, WeightHistoryPointRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        
        services.AddScoped<IAccountQueries, AccountQueries>();
        services.AddScoped<IExerciseProposalQueries, ExerciseProposalQueries>();
        services.AddScoped<IExerciseQueries, ExerciseQueries>();
        services.AddScoped<IPersonalHealthAccountQueries, PersonalHealthAccountQueries>();
        services.AddScoped<IWorkoutQueries, WorkoutQueries>();
        
        return services;
    }
}