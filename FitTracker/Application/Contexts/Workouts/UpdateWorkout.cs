using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;
using Application.Exceptions;
using Domain.Repositories;
using Domain.Services;
using Domain.ValueObjects;
using MediatR;

namespace Application.Workouts;

public record UpdateWorkoutCommand (
    Guid WorkoutId,
    List<UpdateWorkoutExerciseItemRequest>? Exercises = null,
    DateTime? Date = null,
    TimeSpan? Duration = null
) : IRequest<Guid>;

public record UpdateWorkoutExerciseItemRequest(
    Guid Id,
    int Repetitions,
    double Weight
);

public class UpdateWorkoutCommandHandler(
    ICurrentUserAccessor currentUserAccessor,
    IUnitOfWork unitOfWork,
    IWorkoutRepository workoutRepository,
    IPersonalHealthAccountRepository personalHealthAccountRepository,
    IExerciseRepository exerciseRepository)
    : IRequestHandler<UpdateWorkoutCommand, Guid>
{ 
    public async Task<Guid> Handle(UpdateWorkoutCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        
        var workout = await workoutRepository.GetByIdAsync(request.WorkoutId, ct)
            ?? throw new NotFoundException("workout", request.WorkoutId);
        var user = await personalHealthAccountRepository.GetByIdAsync(currentUser.Id, ct)
                   ?? throw new BadRequestException("User health profile not found.");

        if (workout.UserId != currentUser.Id)
        {
            throw new ForbiddenAccessException();
        }

        if (request.Date is not null)
        {
            workout.ChangeDate(request.Date.Value);
        }

        if (request.Duration is not null)
        {
            workout.ChangeDuration(request.Duration.Value);
        }

        if (request.Exercises is not null)
        {
            workout.RemoveExercises();
            
            var exercisesIds = request.Exercises
                .Select(x => x.Id)
                .Distinct()
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
        }
        
        await unitOfWork.SaveChangesAsync(ct);
        return workout.Id;
    }
}