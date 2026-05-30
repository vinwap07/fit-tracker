using Application.PersonalHealth;
using FluentValidation;

namespace Application.Validators;

public class UpdatePersonalHealthAccountCommandValidator: AbstractValidator<UpdatePersonalHealthAccountCommand>
{
    public UpdatePersonalHealthAccountCommandValidator()
    {
        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .When(x => x.DateOfBirth.HasValue);
        RuleFor(x => x.Height)
            .GreaterThan(0)
            .When(x => x.Height.HasValue);
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .When(x => x.Weight.HasValue);
    }
}