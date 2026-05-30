using Domain.Aggregates;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.ValueObjects;

public record UserProfile
{
    public Height Height { get; init; }
    public Weight Weight { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public Sex Sex { get; init; }

    protected UserProfile() { }
    internal UserProfile(Height height, Weight weight, DateOnly dateOfBirth, Sex sex)
    {
        if (DateTime.Now.Year - dateOfBirth.Year < 16)
        {
            throw new DomainException("User must be older than 16 years old.");
        }
        
        Height = height;
        Weight = weight;
        DateOfBirth = dateOfBirth;
        Sex = sex;
    }
}

public enum Sex
{
    Male = 0,
    Female = 1
}