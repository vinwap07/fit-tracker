using Domain.Exceptions;
using Domain.ValueObjects;
using Domain.Entities;
using Domain.Services;

namespace Domain.Aggregates;

public class Workout: IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan Duration { get; private set; }
    public double TotalCalories { get; private set; }
    
    private readonly List<WorkoutExercise> _exercises = new();
    public IReadOnlyCollection<WorkoutExercise> Exercises => _exercises.AsReadOnly();

    protected Workout() {}
    public Workout(Guid userId, DateTime date, TimeSpan duration)
    {
        if (duration < TimeSpan.Zero)
        {
            throw new DomainException("Duration cannot be less than zero.");
        }

        if (date > DateTime.Now)
        {
            throw new DomainException("Date cannot be in future.");
        }
        
        Id = Guid.NewGuid();
        UserId = userId;
        Date = date;
        Duration = duration;
    }

    public void AddExercise(Guid exerciseId, ExerciseVolume volume, double totalCalories)
    {
        if (volume.Repetitions <= 0)
        {
            throw new DomainException("Exercise reps must be greater than zero.");
        }
        
        _exercises.Add(new WorkoutExercise(exerciseId, volume, totalCalories));
        RecalculateTotalCalories();
    }

    public void DeleteExercise(Guid exerciseId)
    {
        var exercise = _exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);

        if (exercise is null)
        {
            throw new DomainException("Exercise does not exist.");
        }
        
        _exercises.Remove(exercise);
        RecalculateTotalCalories();
    }

    public void EditExercise(Guid exerciseId, ExerciseVolume volume)
    {
        var exercise = _exercises.FirstOrDefault(e => e.ExerciseId == exerciseId);

        if (exercise is null)
        {
            throw new DomainException("Exercise does not exist.");
        }
        
        exercise.UpdateVolume(volume);
        RecalculateTotalCalories();
    }

    public void RemoveExercises()
    {
        _exercises.Clear();
        RecalculateTotalCalories();
    }

    public void ChangeDuration(TimeSpan time)
    {
        Duration = time;
    }

    public void ChangeDate(DateTime date)
    {
        if (date > DateTime.Now)
        {
            throw new DomainException("Date cannot be in future.");
        }
        
        Date = date;
    }

    private void RecalculateTotalCalories()
        => TotalCalories = _exercises.Sum(e => e.TotalCalories);
}