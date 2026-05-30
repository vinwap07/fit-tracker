using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.Abstractions.Queries;
using Application.DTOs.UserHealth;
using MediatR;

namespace Application.PersonalHealth;

public record GetPersonalHealthAccountQuery : IRequest<PersonalHealthAccountDto>;

public class GetPersonalHealthAccountQueryHandler(
    ICurrentUserAccessor currentUserAccessor,
    IPersonalHealthAccountQueries personalHealthAccountQueries
    
    ) : IRequestHandler<GetPersonalHealthAccountQuery, PersonalHealthAccountDto>
{
    public async Task<PersonalHealthAccountDto?> Handle(GetPersonalHealthAccountQuery request, CancellationToken ct)
    {
        var currentUser = currentUserAccessor.GetCurrentUser();
        return await personalHealthAccountQueries.GetByIdAsync(currentUser.Id, ct); 
    }
}

