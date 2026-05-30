namespace Application.DTOs.Auth;

public record LoginResult(Guid UserId, string Token, DateTime ExpireAt , string? ErrorMessage = null);