using FluentValidation;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerValidator
    : AbstractValidator<CreateAnswerRequest>
{
    public CreateAnswerValidator()
    {
        RuleFor(x => x.QuestionId).NotEmpty();

        RuleFor(x => x.Content).NotEmpty();
    }
}