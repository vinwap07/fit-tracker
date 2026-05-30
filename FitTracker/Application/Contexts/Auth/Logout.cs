using System.Windows.Input;
using Application.Abstractions.Auth;
using Application.DTOs.Auth;
using MediatR;

namespace Application.Auth;

public record LogoutCommand(string Token) : IRequest;

public class LogoutCommandHandler(
    IIdentityService identityService
    ) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken ct)
    {
        await identityService.LogoutAsync(request.Token, ct);
    }
}