using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public class WeightHistoryPoint: IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; private set; }
    public Weight Weight { get; private set; }

    protected WeightHistoryPoint() {}
    public WeightHistoryPoint(Guid userId, Weight weight, DateOnly? date)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        
        date ??= DateOnly.FromDateTime(DateTime.Now);
        
        Date = date.Value;

        if (weight.Kilograms <= 0)
        {
            throw new DomainException("Weight must be greater than 0");
        }
        
        Weight = weight;
    }
}