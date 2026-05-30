using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Auth;
using Domain.Aggregates;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class TokenService(IOptions<JwtOptions> jwtOptions): ITokenService
{
    private readonly JwtOptions _options = jwtOptions.Value;
    
    public (string, DateTime) GenerateToken(UserAccount userAccount)
    {
        var handler = new JwtSecurityTokenHandler();
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userAccount.Id.ToString()),
            new(ClaimTypes.Email, userAccount.Email),
            new(ClaimTypes.Name, userAccount.Login),
            new(ClaimTypes.Role, userAccount.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return (handler.WriteToken(token), expiresAtUtc);
    }

    public TokenMetadata GetTokenMetadata(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var jti = jwtToken.Claims.FirstOrDefault(c => 
            c.Type == JwtRegisteredClaimNames.Jti || 
            c.Type == "jti")?.Value;
        var expiry = jwtToken.ValidTo;
        
        return new TokenMetadata(jti ?? string.Empty, expiry);
    }
}