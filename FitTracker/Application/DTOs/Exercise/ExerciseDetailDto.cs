namespace Application.DTOs.Exercise;

public class ExerciseDetailDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public double Met { get; init; }
    public string PerformanceVideoUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}