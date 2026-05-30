using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;
using Application.Exceptions;
using Domain.Aggregates;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using MediatR;

namespace Application.Workouts;

public record CreateWorkoutCommand(
    List<CreateWorkoutExerciseItemRequest> Exercises,
    DateTime Date,
    TimeSpan Duration
) : IRequest<Guid>;

public record CreateWorkoutExerciseItemRequest(
    Guid Id,
    int Repetitions,
    double Weight
);

public class CreateWorkoutCommandHandler(
    ICurrentUserAccessor currentUserAccessor,
    IUnitOfWork unitOfWork,
    IWorkoutRepository workoutRepository,
    IPersonalHealthAccountRepository personalHealthAccountRepository,
    IExerciseRepository exerciseRepository)
    : IRequestHandler<CreateWorkoutCommand, Guid>
{
    public async Task<Guid> Handle(CreateWorkoutCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        var workout = new Workout(currentUser.Id, request.Date, request.Duration);
        var user = await personalHealthAccountRepository.GetByIdAsync(currentUser.Id, ct)
                   ?? throw new BadRequestException("User health profile not found.");

        var exercisesIds = request.Exercises
            .Select(x => x.Id)
            .ToList();
        var exercises = await exerciseRepository.GetByIdsAsync(exercisesIds, ct);
        var exercisesDict = exercises.ToDictionary(e => e.Id);
        
        foreach (var exerciseRequest in request.Exercises)
        {
            if (!exercisesDict.TryGetValue(exerciseRequest.Id, out var exercise))
            {
                throw new NotFoundException("exercise", exerciseRequest.Id);
            }
            
            var volume = new ExerciseVolume(exerciseRequest.Repetitions, exerciseRequest.Weight);
            var totalCalories = CalorieCalculationService.CalculateExerciseCalories(
                exercise.Met, volume, user.Profile);
            workout.AddExercise(exercise.Id, volume, totalCalories);
        }
        
        await workoutRepository.SaveAsync(workout, ct);
        await  unitOfWork.SaveChangesAsync(ct);
        
        return workout.Id;
    }
}