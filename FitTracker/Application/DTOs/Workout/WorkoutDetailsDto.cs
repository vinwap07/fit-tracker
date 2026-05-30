namespace Application.DTOs.Workout;

public class WorkoutDetailsDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public double TotalCalories { get; set; }
    public List<WorkoutExerciseItemDto> Exercises { get; set; } = new();
}

public class WorkoutExerciseItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Repetitions { get; set; }
    public double Weight { get; set; }
}