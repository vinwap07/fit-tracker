using Domain.Exceptions;

namespace Domain.ValueObjects;

public record ExerciseVolume
{
    public int Repetitions { get; init; }
    public Weight Weight { get; init; }
    
    protected ExerciseVolume() { }
    public ExerciseVolume(int repetitions, double weight)
    {
        if (weight < 0)
        {
            throw new DomainException("Weight cannot be negative");
        }

        if (repetitions <= 0)
        {
            throw new DomainException("Repetitions cannot be less or equal to zero");
        }
        
        Weight = new Weight(weight);
        Repetitions = repetitions;
    }
}