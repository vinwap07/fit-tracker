using Application.Exercises.Proposals;
using FluentValidation;

namespace Application.Validators;

public class SendExerciseProposalToRevisionCommandValidator: AbstractValidator<SendExerciseProposalToRevisionCommand>
{
    public SendExerciseProposalToRevisionCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty();
    }
}