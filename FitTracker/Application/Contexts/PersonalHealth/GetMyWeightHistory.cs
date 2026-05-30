using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.UserHealth;
using MediatR;

namespace Application.PersonalHealth;

public record GetMyWeightHistoryQuery : IRequest<List<WeightHistoryListItemDto>>;

public class GetMyWeightHistoryQueryHandler(
    ICurrentUserAccessor userAccessor,
    IPersonalHealthAccountQueries personalHealthAccountQueries
    ) : IRequestHandler<GetMyWeightHistoryQuery, List<WeightHistoryListItemDto>>
{
    public async Task<List<WeightHistoryListItemDto>> Handle(GetMyWeightHistoryQuery request, CancellationToken ct)
    {
        var currentUser = userAccessor.GetCurrentUser();
        return await personalHealthAccountQueries.GetUsersWeightHistoryAsync(currentUser.Id, ct);
    }
}