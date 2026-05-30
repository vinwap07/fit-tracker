using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Exercises.Proposals;

public record DeleteExerciseProposalCommand(Guid ProposalId): IRequest<Guid>;

public class DeleteExerciseProposalCommandHandler(
    ICurrentUserAccessor userAccessor,
    IExerciseProposalRepository exerciseProposalRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteExerciseProposalCommand, Guid>
{
    public async Task<Guid> Handle(DeleteExerciseProposalCommand request, CancellationToken ct)
    {
        var currentUser = userAccessor.GetCurrentUser();
        var proposal = await exerciseProposalRepository.GetByIdAsync(request.ProposalId, ct)
            ?? throw new NotFoundException("exercise proposal", request.ProposalId);

        if (currentUser.Id != proposal.AuthorId)
        {
            throw new ForbiddenAccessException();
        }
        
        await exerciseProposalRepository.DeleteAsync(proposal, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return proposal.Id;
    }
}