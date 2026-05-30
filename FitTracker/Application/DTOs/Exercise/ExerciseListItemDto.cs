namespace Application.DTOs.Exercise;

public class ExerciseListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PhotoUrl { get; init; } = string.Empty;
}