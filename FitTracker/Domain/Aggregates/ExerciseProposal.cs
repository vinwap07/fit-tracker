using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Aggregates;

public class ExerciseProposal: IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid AuthorId { get; private set; }
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Media Photo { get; private set; }
    public Media PerformanceVideo { get; private set; }
    public Media EmgVideo { get; private set; }
    
    public double Met { get; private set; }
    public string Comment { get; private set; } = string.Empty;
    
    public ProposalStatus Status { get; private set; }

    protected ExerciseProposal() {}
    public ExerciseProposal(Guid authorId, string name, string description, Media photo, Media performanceVideo, Media emgVideo, double met)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name cannot be null or empty");
        }

        if (authorId == Guid.Empty)
        {
            throw new DomainException("AuthorId cannot be empty");
        }
        
        Id = Guid.NewGuid();
        AuthorId = authorId;
        Name = name;
        Description = description;
        Photo = photo;
        PerformanceVideo = performanceVideo;
        EmgVideo = emgVideo;
        Met = met;
        Status = ProposalStatus.Pending;
    }

    public void Approve()
    {
        if (Status == ProposalStatus.Rejected)
        {
            throw new DomainException("Proposal is rejected");
        }
        
        Status = ProposalStatus.Approved;
    }

    public void Reject(string? comment)
    {
        if (Status == ProposalStatus.Approved)
        {
            throw new DomainException("Proposal is approved");
        }
        
        Status = ProposalStatus.Rejected;
        Comment = comment ?? string.Empty;
    }

    public void SendBackToRevision(string comment)
    {
        if (Status != ProposalStatus.Pending)
        {
            throw new DomainException("Proposal already processed");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new DomainException("Needs comment to be sent back for revision");
        }

        Status = ProposalStatus.NeedsRevision;
        Comment = comment;
    }

    public void UpdateInformation(string? name, string? description, double? met)
    {
        Status = ProposalStatus.Pending;
        Name = name ?? Name;
        Description = description ?? Description;
        Met = met ?? Met;
    }

    public void UpdatePerformanceVideo(Media performanceVideo)
    {
        PerformanceVideo = performanceVideo;
    }
    
    public void UpdateEmgVideo(Media emgVideo)
    {
        EmgVideo = emgVideo;
    }
    
    public void UpdatePhoto(Media photo)
    {
        Photo = photo;
    }
}

public enum ProposalStatus
{
    Pending,
    Approved,
    Rejected,
    NeedsRevision
}