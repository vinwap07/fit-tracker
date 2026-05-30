using Application.Abstractions.Auth;
using Application.Auth;
using Application.DTOs.Auth;
using Destructurama.Attributed;
using MediatR;

namespace Application.Auth;

public record RegisterCommand: IRequest<LoginResult>
{
    public required string Email { get; init; }
    public required string Login { get; init; }
    
    [NotLogged] 
    public required string Password { get; init; }
}

public class RegisterCommandHandler(
    IIdentityService identityService
    ) : IRequestHandler<RegisterCommand, LoginResult>
{
    public async Task<LoginResult> Handle(RegisterCommand request, CancellationToken ct)
    {
        return await identityService.RegisterAsync(request.Email, request.Login, request.Password, ct);
    }
}