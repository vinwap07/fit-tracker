using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Exceptions;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Exercises.Proposals;

public record SendExerciseProposalToRevisionCommand(Guid ProposalId, string Comment) : IRequest;

public class SendExerciseProposalToRevisionCommandHandler(
    ILogger<SendExerciseProposalToRevisionCommandHandler> logger,
    ICurrentUserAccessor currentUserAccessor,
    IExerciseProposalRepository exerciseProposalRepository,
    IUserAccountRepository userAccountRepository,
    IJobService jobService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SendExerciseProposalToRevisionCommand>
{
    public async Task Handle(SendExerciseProposalToRevisionCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        
        var exerciseProposal = await exerciseProposalRepository.GetByIdAsync(request.ProposalId, ct)
                               ?? throw new NotFoundException("exercise proposal", request.ProposalId);

        if (exerciseProposal.AuthorId == currentUser.Id)
        {
            throw new BadRequestException("Moderator can't reject own exercise proposal");
        }
        
        exerciseProposal.SendBackToRevision(request.Comment);
        
        var author = await userAccountRepository.GetByIdAsync(exerciseProposal.AuthorId, ct)
                     ?? throw new NotFoundException("UserAccount", currentUser.Id);
            
        await unitOfWork.SaveChangesAsync(ct);
        
        logger.LogInformation("Moderator {ModeratorId} is sending back to revision proposal {ProposalId}", 
            currentUser.Id, request.ProposalId);
        
        jobService.Enqueue<IEmailService>(emailService => 
            emailService.SendEmailAsync(author.Email, "Заявка на создание упражнения", 
                $"Ваша заявка на создание упражнения {exerciseProposal.Name} была отправлена на доработку: {request.Comment}."));
    }
}