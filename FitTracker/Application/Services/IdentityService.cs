using Application.Abstractions;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;
using Domain.Aggregates;
using Domain.Repositories;
using Domain;

namespace Application.Services;

public class IdentityService(
    IUserAccountRepository userAccountRepository,
    IUnitOfWork unitOfWork,
    ITokenService tokenService,
    ITokenBlacklistService tokenBlacklistService,
    IPasswordHasher passwordHasher
    ): IIdentityService
{
    public async Task<LoginResult> RegisterAsync(string email, string login, string password, CancellationToken ct = default)
    {
        var existingAccount = await userAccountRepository.GetByEmailAsync(email, ct);
        if (existingAccount != null)
        {
            return new LoginResult(Guid.Empty, string.Empty, DateTime.Now,"User already exists");
        }
        
        var hash = passwordHasher.Hash(password);
        var user = new UserAccount(email, login, hash);
        await userAccountRepository.SaveAsync(user, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        var tokenData = tokenService.GenerateToken(user);
        return new LoginResult(user.Id, tokenData.Item1, tokenData.Item2);
    }

    public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var existingAccount = await userAccountRepository.GetByEmailAsync(email, ct);
        if (existingAccount == null)
        {
            return new LoginResult(Guid.Empty, string.Empty, DateTime.Now,"Email or password is incorrect");
        }
        
        var isPasswordCorrect = existingAccount.VerifyPassword(password, passwordHasher);

        if (!isPasswordCorrect)
        {
            return new LoginResult(Guid.Empty, string.Empty, DateTime.Now, "Email or password is incorrect");
        }
        
        var tokenData = tokenService.GenerateToken(existingAccount);
        return new LoginResult(existingAccount.Id, tokenData.Item1, tokenData.Item2);
    }

    public async Task LogoutAsync(string token, CancellationToken ct = default)
    {
        var metadata = tokenService.GetTokenMetadata(token);
        await tokenBlacklistService.BanAsync(metadata.Jti, metadata.Expiry, ct);
    }
}