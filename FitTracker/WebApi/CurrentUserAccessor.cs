using System.Security.Claims;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;

namespace Presentation;

public class CurrentUserAccessor: ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            return new CurrentUser
            {
                IsAuthenticated = false
            };
        }

        var idRaw = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return new CurrentUser
        {
            Id = Guid.TryParse(idRaw, out var id) ? id : Guid.Empty,
            Email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Login = user.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            Role = user.FindFirstValue(ClaimTypes.Role) ?? string.Empty,
            IsAuthenticated = true
        };
    }
}