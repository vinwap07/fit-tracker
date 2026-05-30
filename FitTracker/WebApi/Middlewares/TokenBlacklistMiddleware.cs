using Application.Abstractions.Auth;
using Application.DTOs.Auth;

namespace Presentation.Middlewares;

public class TokenBlacklistMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ITokenBlacklistService blacklistService,
        ITokenService tokenService,
        ILogger<TokenBlacklistMiddleware> logger)
    {
        if (context.User.Identity is { IsAuthenticated: true })
        {
            var token = context.Request.Cookies["jwt-token"];

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var metadata = tokenService.GetTokenMetadata(token);

                    if (await blacklistService.IsBannedAsync(metadata.Jti))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new
                            { message = "Session expired. Please log in again." });
                        return;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex,
                        "Token blacklist check failed; continuing without blocking the request.");
                }
            }
        }

        await next(context);
    }
}