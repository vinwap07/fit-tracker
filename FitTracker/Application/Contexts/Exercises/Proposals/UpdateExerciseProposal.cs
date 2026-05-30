using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Exceptions;
using Application.Workouts.GetWorkoutInformation;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Exercises.Proposals;

public record UpdateExerciseProposalCommand(
    Guid Id,
    string? Name,
    string? Description,
    double? Met,
    FileParameter? PerformanceVideo,
    FileParameter? EmgVideo,
    FileParameter? Photo
    ) : IRequest<Guid>;
    
    
public class UpdateExerciseProposalCommandHandler(
    ICurrentUserAccessor userAccessor,
    IUnitOfWork unitOfWork,
    IExerciseProposalRepository exerciseProposalRepository,
    IFileStorageService fileStorageService
    ) : IRequestHandler<UpdateExerciseProposalCommand, Guid>
{
    public async Task<Guid> Handle(UpdateExerciseProposalCommand request, CancellationToken ct)
    {
        var currentUser = userAccessor.GetCurrentUser();
        var proposal = await exerciseProposalRepository.GetByIdAsync(request.Id, ct)
            ?? throw new NotFoundException("exercise proposal", request.Id);

        if (currentUser.Id != proposal.AuthorId)
        {
            throw new ForbiddenAccessException();
        }

        if (request.PerformanceVideo is not null)
        {
            var performanceVideo = await ReplaceFileAsync(proposal.PerformanceVideo, request.PerformanceVideo, ct);
            proposal.UpdatePerformanceVideo(performanceVideo);
        }
        
        if (request.EmgVideo is not null)
        {
            var emgVideo = await ReplaceFileAsync(proposal.EmgVideo, request.EmgVideo, ct);
            proposal.UpdateEmgVideo(emgVideo);
        }
        
        if (request.Photo is not null)
        {
            var photo = await ReplaceFileAsync(proposal.Photo, request.Photo, ct);
            proposal.UpdatePhoto(photo);
        }
        
        proposal.UpdateInformation(request.Name, request.Description, request.Met);
        await unitOfWork.SaveChangesAsync(ct);
        return proposal.Id;
    }
    
    private async Task<Media> ReplaceFileAsync(Media oldFile, FileParameter newFile, CancellationToken ct)
    {
        var uploadResult = await fileStorageService.UploadFileAsync(
            newFile.Content,
            newFile.FileName,
            newFile.ContentType,
            // TODO: изменить название бакета на настраиваемое из конфига
            string.Empty,
            ct);
        await fileStorageService.DeleteFileAsync(oldFile.Bucket, oldFile.Key, ct);
        return uploadResult;
    }
}