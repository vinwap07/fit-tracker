using Application.Exercises.Proposals;
using FluentValidation;

namespace Application.Validators;

public class UpdateExerciseProposalCommandValidator: AbstractValidator<UpdateExerciseProposalCommand>
{
    public UpdateExerciseProposalCommandValidator()
    {
        RuleFor(x => x.Met)
            .GreaterThan(0);
    }
}