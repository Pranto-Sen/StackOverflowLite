//using MediatR;

//namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

//public record UnacceptAnswerCommand(Guid QuestionId) : IRequest;


using MediatR;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public class UnacceptAnswerCommand : IRequest
{
    public Guid AnswerId { get; set; }

    public UnacceptAnswerCommand(Guid answerId)
    {
        AnswerId = answerId;
    }
}