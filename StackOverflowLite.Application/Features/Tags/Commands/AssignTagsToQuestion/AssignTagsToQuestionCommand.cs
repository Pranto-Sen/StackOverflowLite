using MediatR;

namespace StackOverflowLite.Application.Features.Tags.Commands.AssignTagsToQuestion;

public class AssignTagsToQuestionCommand: IRequest
{
    public Guid QuestionId { get; set; }

    public List<Guid> TagIds { get; set; } = new();
}