using Application.Abstractions;
using Application.Abstractions.Auth;
using Domain.Aggregates;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.PersonalHealth;

public record CreatePersonalHealthAccountCommand(
    int Height,
    double Weight,
    DateOnly DateOfBirth,
    Sex Sex
    ) : IRequest<Guid>;
    
public class CreatePersonalHealthProfileAccountHandler(
    ICurrentUserAccessor currentUserAccessor,
    IPersonalHealthAccountRepository personalHealthAccountRepository,
    IWeightHistoryPointRepository weightHistoryPointRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CreatePersonalHealthAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreatePersonalHealthAccountCommand request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();

        var weight = new Weight(request.Weight);
        var height = new Height(request.Height);
        
        var healthAccount = new PersonalHealthAccount(
            currentUser.Id,
            height,
            weight,
            request.DateOfBirth,
            request.Sex);

        var weightHistory = new WeightHistoryPoint(currentUser.Id, weight, DateOnly.FromDateTime(DateTime.Now));
        
        await personalHealthAccountRepository.SaveAsync(healthAccount, ct);
        await weightHistoryPointRepository.SaveAsync(weightHistory, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return healthAccount.Id;
    }
}