using System.Data;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public record Weight
{
    public double Kilograms { get; init; }

    protected Weight() { }
    public Weight(double kilograms)
    {
        if (kilograms <= 0)
        {
            throw new DomainException("Kilograms must be greater than 0");
        }
        
        Kilograms = kilograms;
    }
    
    public override string ToString() => $"{Kilograms} кг";
}