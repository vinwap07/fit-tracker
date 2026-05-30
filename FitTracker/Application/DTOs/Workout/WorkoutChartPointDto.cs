namespace Application.DTOs.Workout;

public class WorkoutChartPointDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeSpan Duration { get; set; }
}