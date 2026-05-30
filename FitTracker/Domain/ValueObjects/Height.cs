using Domain.Exceptions;

namespace Domain.ValueObjects;

public record Height
{
    public int Centimeters { get; init; }

    protected Height() {}
    public Height(int centimeters)
    {
        if (centimeters <= 50)
        {
            throw new DomainException("Height must be greater than 50");
        }
        
        Centimeters = centimeters;
    }
    
    public override string ToString() => $"{Centimeters} см";
}