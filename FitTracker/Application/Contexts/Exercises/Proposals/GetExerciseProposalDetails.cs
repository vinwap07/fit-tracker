using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.DTOs.ExerciseProposal;
using Application.Exceptions;
using Application.Workouts.GetWorkoutInformation;
using AutoMapper;
using MediatR;

namespace Application.Exercises.Proposals;

public record GetExerciseProposalDetailsQuery(Guid ProposalId) : IRequest<ExerciseProposalDetailsDto>;

public class GetExerciseProposalDetailsQueryHandler(
    IExerciseProposalQueries exerciseProposalQueries,
    IFileStorageService fileStorageService,
    IMapper mapper,
    ICurrentUserAccessor currentUserAccessor
    ) : IRequestHandler<GetExerciseProposalDetailsQuery, ExerciseProposalDetailsDto>
{
    public async Task<ExerciseProposalDetailsDto> Handle(GetExerciseProposalDetailsQuery request, CancellationToken ct)
    {
        var user = currentUserAccessor.GetCurrentUser();
        var proposal = await exerciseProposalQueries.GetByIdAsync(request.ProposalId, ct) 
                       ?? throw new NotFoundException("exercise proposal", request.ProposalId);;

        // TODO: переделать роли на взятые из конфига
        var isModerator = user.Role == "Moderator";
        var isOwner = user.Id == proposal.AuthorId;

        if (!isModerator && !isOwner)
        {
            throw new ForbiddenAccessException();
        }

        proposal.PhotoUrl = await fileStorageService.GetPresignedUrlAsync(proposal.Bucket, proposal.PhotoKey);
        proposal.PerformanceVideoUrl = await fileStorageService.GetPresignedUrlAsync(proposal.Bucket, proposal.PerformanceVideoKey);
        proposal.EmgVideoUrl = await fileStorageService.GetPresignedUrlAsync(proposal.Bucket, proposal.EmgVideoKey);
        
        return mapper.Map<ExerciseProposalDetailsDto>(proposal);
    }
}