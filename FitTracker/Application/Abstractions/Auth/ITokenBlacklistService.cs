namespace Application.Abstractions.Auth;

public interface ITokenBlacklistService
{
    Task BanAsync(string token, DateTime expiry, CancellationToken ct = default);
    
    Task<bool> IsBannedAsync(string token, CancellationToken ct = default);
}