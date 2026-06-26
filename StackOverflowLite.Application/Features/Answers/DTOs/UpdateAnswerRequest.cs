using MediatR;

namespace StackOverflowLite.Application.Features.Answers.DTOs;

public class UpdateAnswerRequest: IRequest
{
    public Guid Id { get; set; }

    public string Content { get; set; } = string.Empty;
}