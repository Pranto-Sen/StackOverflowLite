using FluentValidation;

namespace StackOverflowLite.Application.Features.Votes.Commands;

public class VoteQuestionValidator: AbstractValidator<VoteQuestionCommand>
{
    public VoteQuestionValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty();
    }
}