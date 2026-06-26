using MediatR;

namespace StackOverflowLite.Application.Features.Questions.DTOs;

public class CreateQuestionRequest: IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

}