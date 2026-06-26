using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands;

public class VoteAnswerValidator
    : AbstractValidator<VoteAnswerCommand>
{
    public VoteAnswerValidator()
    {
        RuleFor(x => x.AnswerId)
            .NotEmpty();
    }
}