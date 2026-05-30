namespace Application.DTOs.Workout;

public class WorkoutCaloriesChartPointDto
{
    public Guid Id { get; set; }
    public double Calories { get; set; }
    public DateOnly Date { get; set; }
}