using Application.Workouts;
using FluentValidation;

namespace Application.Validators;

public class CreateWorkoutCommandValidator: AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Duration)
            .NotEmpty()
            .GreaterThan(TimeSpan.Zero)
            .LessThanOrEqualTo(TimeSpan.FromDays(1));
        RuleFor(x => x.Exercises)
            .NotEmpty();
    }
}