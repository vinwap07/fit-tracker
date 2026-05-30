using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Exceptions;
using Domain.Aggregates;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Exercises;

public record ApproveExerciseProposalCommand(Guid ProposalId) : IRequest;

public class ApproveExerciseProposalCommandHandler(
    ILogger<ApproveExerciseProposalCommandHandler> logger, 
    ICurrentUserAccessor currentUserAccessor,
    IExerciseProposalRepository exerciseProposalRepository,
    IExerciseRepository exerciseRepository,
    IUserAccountRepository userAccountRepository,
    IUnitOfWork unitOfWork,
    IJobService jobService
    ) : IRequestHandler<ApproveExerciseProposalCommand>
{
    public async Task Handle(ApproveExerciseProposalCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        
        var exerciseProposal = await exerciseProposalRepository.GetByIdAsync(request.ProposalId, ct)
            ?? throw new NotFoundException("exercise proposal", request.ProposalId);

        if (exerciseProposal.AuthorId == currentUser.Id)
        {
            throw new BadRequestException("Moderator can't approve own exercise proposal");
        }
        exerciseProposal.Approve();
        var author = await userAccountRepository.GetByIdAsync(exerciseProposal.AuthorId, ct)
            ?? throw new NotFoundException("UserAccount", exerciseProposal.AuthorId);

        var exercise = new Exercise(exerciseProposal.Name,
            exerciseProposal.Description,
            exerciseProposal.Met,
            exerciseProposal.Photo,
            exerciseProposal.PerformanceVideo);
        await exerciseRepository.SaveAsync(exercise, ct);
        
        await unitOfWork.SaveChangesAsync(ct);
        
        logger.LogInformation("Moderator {ModeratorId} is approving proposal {ProposalId}", 
            currentUser.Id, request.ProposalId);
        
        jobService.Enqueue<IEmailService>(emailService => 
            emailService.SendEmailAsync(author.Email, "Заявка на создание упражнения", 
                $"Ваша заявка на создание упражнения {exerciseProposal.Name} была одобрена."));
    }
}
