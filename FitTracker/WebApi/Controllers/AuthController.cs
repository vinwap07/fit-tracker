using Application.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

namespace Presentation.Controllers;

public class AuthController: BaseApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
            return BadRequest(result.ErrorMessage);
        }
        SetTokenCookie(result.Token, result.ExpireAt);
        return result.ErrorMessage is null ? Ok(result) : BadRequest(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        SetTokenCookie(result.Token, result.ExpireAt);
        return result.ErrorMessage is null ? Ok(result) : BadRequest(result);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand? command, CancellationToken ct)
    {
        var token = Request.Cookies["jwt-token"] ?? command?.Token ?? string.Empty;
        if (!string.IsNullOrEmpty(token))
            await Mediator.Send(new LogoutCommand(token), ct);

        Response.Cookies.Delete("jwt-token");
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMeAsync([FromQuery] GetMyAccountInfoQuery query, CancellationToken ct)
    {
        var result = await Mediator.Send(query, ct);
        return Ok(result);
    }
    
    private void SetTokenCookie(string token, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = expires
        };

        Response.Cookies.Append("jwt-token", token, cookieOptions);
    }
}