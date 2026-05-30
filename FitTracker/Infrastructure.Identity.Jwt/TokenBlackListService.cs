using Application.Abstractions;
using Application.Abstractions.Auth;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Identity;

public class TokenBlackListService(IDistributedCache cache): ITokenBlacklistService
{
    public async Task BanAsync(string token, DateTime expiry, CancellationToken ct = default)
    {
        await cache.SetStringAsync(token, "revoked", new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expiry
        }, ct);
    }

    public async Task<bool> IsBannedAsync(string token, CancellationToken ct = default)
    {
        var result = await cache.GetStringAsync(token, ct);
        return result != null;
    }
}