using Domain.Aggregates;

namespace Application.Abstractions.Auth;

public record TokenMetadata(string Jti, DateTime Expiry);

public interface ITokenService
{
    (string, DateTime) GenerateToken(UserAccount userAccount);
    
    TokenMetadata GetTokenMetadata(string token);
}