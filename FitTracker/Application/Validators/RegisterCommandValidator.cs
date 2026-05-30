using Application.Auth;
using FluentValidation;

namespace Application.Validators;

public class RegisterCommandValidator: AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(x => x.Login)
            .NotEmpty()
            .MinimumLength(5);
    }
}