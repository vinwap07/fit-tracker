using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;
using Application.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Workouts;

public record DeleteWorkoutCommand(Guid Id) : IRequest<Guid>;

public class DeleteWorkoutCommandHandler(
    ICurrentUserAccessor currentUserAccessor,
    IUnitOfWork unitOfWork,
    IWorkoutRepository workoutRepository)
    : IRequestHandler<DeleteWorkoutCommand, Guid>
{
    public async Task<Guid> Handle(DeleteWorkoutCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        var workout = await workoutRepository.GetByIdAsync(request.Id, ct);
        if (workout == null)
        {
            return request.Id;
        }
        
        if (workout.UserId != currentUser.Id)
        {
            throw new ForbiddenAccessException();
        }

        await workoutRepository.DeleteAsync(workout, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return request.Id;
    }
}