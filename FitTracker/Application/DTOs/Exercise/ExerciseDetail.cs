namespace Application.DTOs.Exercise;

public class ExerciseDetail
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public double Met { get; init; }
    public string PerformanceVideoKey { get; init; } = string.Empty;
    public string PhotoKey { get; init; } = string.Empty;
    public string Bucket { get; init; } = string.Empty;
    public string PerformanceVideoUrl { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}