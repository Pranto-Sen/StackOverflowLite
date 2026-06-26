using MediatR;

namespace StackOverflowLite.Application.Features.Answers.DTOs;

public class AcceptAnswerRequest : IRequest
{
    public Guid AnswerId { get; set; }
}