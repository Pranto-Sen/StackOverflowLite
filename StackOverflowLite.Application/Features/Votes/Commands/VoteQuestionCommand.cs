using MediatR;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands;

public class VoteQuestionCommand: IRequest
{
    public Guid QuestionId { get; set; }

    public VoteType VoteType { get; set; }
}