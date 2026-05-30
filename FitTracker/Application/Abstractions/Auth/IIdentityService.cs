using Application.DTOs.Auth;

namespace Application.Abstractions.Auth;

public interface IIdentityService
{
    Task<LoginResult> RegisterAsync(string email, string login, string password, CancellationToken ct = default);
    
    Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default);
    
    Task LogoutAsync(string token, CancellationToken ct = default);
}