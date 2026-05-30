namespace Application.DTOs.UserHealth;

public class WeightHistoryListItemDto
{
    public Guid Id { get; set; }
    public double Weight { get; set; }
    public DateOnly Date { get; set; }
}