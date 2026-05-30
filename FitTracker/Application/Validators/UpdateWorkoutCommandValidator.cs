using Application.Workouts;
using FluentValidation;

namespace Application.Validators;

public class UpdateWorkoutCommandValidator: AbstractValidator<UpdateWorkoutCommand>
{
    public UpdateWorkoutCommandValidator()
    {
        RuleFor(x => x.Date)
            .LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero)
            .LessThanOrEqualTo(TimeSpan.FromDays(1));
    }
}