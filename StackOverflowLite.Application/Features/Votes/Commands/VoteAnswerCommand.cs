using MediatR;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands;

public class VoteAnswerCommand : IRequest
{
    public Guid AnswerId { get; set; }

    public VoteType VoteType { get; set; }
}