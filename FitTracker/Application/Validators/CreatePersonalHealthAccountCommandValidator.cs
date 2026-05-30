using Application.PersonalHealth;
using FluentValidation;

namespace Application.Validators;

public class CreatePersonalHealthAccountCommandValidator: AbstractValidator<CreatePersonalHealthAccountCommand>
{
    public CreatePersonalHealthAccountCommandValidator()
    {
        RuleFor(x => x.Height)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Weight)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(x => x.Sex).IsInEnum();
    }
}