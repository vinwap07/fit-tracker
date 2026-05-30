using Application.DTOs.Auth;

namespace Application.Abstractions.Auth;

public interface ICurrentUserAccessor
{
    CurrentUser GetCurrentUser();
}