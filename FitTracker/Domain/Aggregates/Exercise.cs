using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public class Exercise: IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public double Met { get; private set; }
    public Media Photo { get; private set; }
    public Media PerformanceVideo { get; private set; }

    protected Exercise() {}
    public Exercise(string name, string description, double met, Media photo, Media performanceVideo)
    {
        if (met <= 0)
        {
            throw new DomainException("Calories per rep must be greater than zero.");
        }
        
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Met = met;
        Photo = photo;
        PerformanceVideo = performanceVideo;
    }

    public void UpdatePhoto(Media photo)
    {
        Photo = photo;
    }

    public void UpdatePerformanceVideo(Media performanceVideo)
    {
        PerformanceVideo = performanceVideo;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new DomainException("Exercise must have a name.");
        }
        
        Name = name;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            throw new DomainException("Exercise must have a description.");
        }
        
        Description = description;
    }

    public void UpdateMet(double met)
    {
        if (met <= 0)
        {
            throw new DomainException("Calories per rep must be greater than zero.");
        }
        
        Met = met;
    }
}