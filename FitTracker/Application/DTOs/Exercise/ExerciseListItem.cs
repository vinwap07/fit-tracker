namespace Application.DTOs.Exercise;

public class ExerciseListItem
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Bucket { get; init; } = string.Empty;
    public string PhotoKey { get; init; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
}