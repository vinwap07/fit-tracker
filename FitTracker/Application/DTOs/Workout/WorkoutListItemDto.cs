namespace Application.DTOs.Workout;

public class WorkoutListItemDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Duration { get; set; }
    public double TotalCalories { get; set; }
}