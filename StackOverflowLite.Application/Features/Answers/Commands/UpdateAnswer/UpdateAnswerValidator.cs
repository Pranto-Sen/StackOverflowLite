using FluentValidation;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerValidator
    : AbstractValidator<CreateAnswerRequest>
{
    public UpdateAnswerValidator()
    {

        RuleFor(x => x.Content).NotEmpty();
    }
}