using Domain.ValueObjects;

namespace Application.DTOs.UserHealth;

public class PersonalHealthAccountDto
{
    public int Height { get; set; }
    public double Weight { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Sex Sex { get; set; }
}