using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.Auth;
using MediatR;

namespace Application.Auth;

public record GetMyAccountInfoQuery : IRequest<AccountInfoDto>;

public class GetMyAccountInfoQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IAccountQueries accountQueries
    ) : IRequestHandler<GetMyAccountInfoQuery, AccountInfoDto>
{
    public async Task<AccountInfoDto?> Handle(GetMyAccountInfoQuery request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        return await accountQueries.GetByIdAsync(currentUser.Id, ct);
    }
}