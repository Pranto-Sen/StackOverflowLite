using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Answers.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Answers.Commands.CreateAnswer;

public class CreateAnswerHandler
    : IRequestHandler<CreateAnswerRequest, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateAnswerHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CreateAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var questionExists = await _context.Questions
            .AnyAsync(
                x => x.Id == request.QuestionId,
                cancellationToken);

        if (!questionExists)
            //throw new Exception("Question not found");
            throw new KeyNotFoundException("Question not found");

        var answer = new Answer
        {
            Id = Guid.NewGuid(),
            QuestionId = request.QuestionId,
            Content = request.Content,
            UserId = _currentUser.UserId!
        };

        _context.Answers.Add(answer);

        await _context.SaveChangesAsync(
            cancellationToken);

        return answer.Id;
    }
}