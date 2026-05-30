using Application.Abstractions;
using Application.Abstractions.Auth;
using Domain.Aggregates;
using Domain.Repositories;
using MediatR;

namespace Application.Exercises.Proposals;

public record ProposeExerciseCommand(
    string Name,
    string Description,
    double Met,
    FileParameter PerformanceVideo,
    FileParameter EmgVideo,
    FileParameter Photo
) : IRequest<Guid>;

public record FileParameter(Stream Content, string FileName, string ContentType);

public class ProposeExerciseCommandHandler(
    IFileStorageService storageService,
    IUnitOfWork unitOfWork,
    IExerciseProposalRepository exerciseProposalRepository,
    ICurrentUserAccessor currentUserAccessor
) : IRequestHandler<ProposeExerciseCommand, Guid>
{
    public async Task<Guid> Handle(ProposeExerciseCommand request, CancellationToken ct)
    {
        var user = currentUserAccessor.GetCurrentUser();
        
        var performanceVideo = await storageService.UploadFileAsync(
            request.PerformanceVideo.Content,
            request.PerformanceVideo.FileName,
            request.PerformanceVideo.ContentType,
            // TODO: заменить название бакета на настраиваемое из конфига
            string.Empty,
            ct);
        var emgVideo = await storageService.UploadFileAsync(
            request.EmgVideo.Content,
            request.EmgVideo.FileName,
            request.EmgVideo.ContentType,
            string.Empty,
            ct);
        var photo = await storageService.UploadFileAsync(
            request.Photo.Content,
            request.Photo.FileName,
            request.Photo.ContentType,
            string.Empty,
            ct);

        var proposal = new ExerciseProposal(user.Id, request.Name, request.Description,
            photo, performanceVideo,emgVideo, request.Met);
        
        await exerciseProposalRepository.SaveAsync(proposal, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return proposal.Id;
    }
}