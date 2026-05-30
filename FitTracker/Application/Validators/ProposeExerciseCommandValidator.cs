using Application.Exercises.Proposals;
using FluentValidation;

namespace Application.Validators;

public class ProposeExerciseCommandValidator: AbstractValidator<ProposeExerciseCommand>
{
    public ProposeExerciseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Description)
            .NotEmpty();
        RuleFor(x => x.Met)
            .NotEmpty()
            .GreaterThan(0);
        RuleFor(x => x.Photo)
            .NotEmpty();
        RuleFor(x => x.PerformanceVideo)
            .NotEmpty();
        RuleFor(x => x.EmgVideo)
            .NotEmpty();
    }
}