using Application.Abstractions.Auth;
using Application.Auth;
using Application.DTOs.Auth;
using Destructurama.Attributed;
using MediatR;

namespace Application.Auth;

public record LoginCommand : IRequest<LoginResult>
{
    public required string Email { get; init; }
    [NotLogged] public required string Password { get; init; }
}

public class LoginCommandHandler(
    IIdentityService identityService
    ) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        return await identityService.LoginAsync(request.Email, request.Password, ct);
    }
}