using MediatR;

namespace StackOverflowLite.Application.Features.Answers.DTOs;

public class CreateAnswerRequest: IRequest<Guid>
{
    public Guid QuestionId { get; set; }

    public string Content { get; set; } = string.Empty;
}