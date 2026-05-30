using Domain.ValueObjects;

namespace Domain.Entities;

public class WorkoutExercise
{
    public Guid ExerciseId { get; private set; }
    public ExerciseVolume Volume { get; private set; }
    public double TotalCalories { get; private set; }

    protected WorkoutExercise() {}
    internal WorkoutExercise(Guid exerciseId, ExerciseVolume exerciseVolume, double totalCalories)
    {
        ExerciseId = exerciseId;
        Volume = exerciseVolume;
        TotalCalories = totalCalories;
    }

    internal void UpdateVolume(ExerciseVolume exerciseVolume)
    {
        Volume = exerciseVolume;
    }
}