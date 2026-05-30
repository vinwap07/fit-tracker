using Application.Auth;
using FluentValidation;

namespace Application.Validators;

public class LoginCommandValidator: AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}