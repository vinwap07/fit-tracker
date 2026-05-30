using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Exceptions;
using Domain.Aggregates;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.PersonalHealth;

public record UpdatePersonalHealthAccountCommand(
    int? Height,
    double? Weight,
    DateOnly? DateOfBirth,
    Sex? Sex
) : IRequest<Guid>;

public class UpdatePersonalHealthAccountCommandHandler(
    ICurrentUserAccessor currentUserAccessor,
    IPersonalHealthAccountRepository personalHealthAccountRepository,
    IWeightHistoryPointRepository weightHistoryPointRepository,
    IUnitOfWork unitOfWork
    ): IRequestHandler<UpdatePersonalHealthAccountCommand, Guid>
{
    public async Task<Guid> Handle(UpdatePersonalHealthAccountCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        var healthAccount = await personalHealthAccountRepository.GetByIdAsync(currentUser.Id, ct)
                            ?? throw new NotFoundException("personal health account", currentUser.Id);
        
        var height = request.Height.HasValue ? new Height(request.Height.Value) : null;
        var weight = request.Weight.HasValue ? new Weight(request.Weight.Value) : null;

        if (weight != null)
        {
            var weightHistoryPoint = new WeightHistoryPoint(currentUser.Id, weight, DateOnly.FromDateTime(DateTime.Now));
            await weightHistoryPointRepository.SaveAsync(weightHistoryPoint, ct);
        }
        
        healthAccount.UpdateProfile(height, weight, request.DateOfBirth, request.Sex);
        await unitOfWork.SaveChangesAsync(ct);
        return healthAccount.Id;
    }
}