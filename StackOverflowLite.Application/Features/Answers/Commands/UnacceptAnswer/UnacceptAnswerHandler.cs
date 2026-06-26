using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;

public class UnacceptAnswerHandler
    : IRequestHandler<UnacceptAnswerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IRedisCacheService _redis;

    public UnacceptAnswerHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IRedisCacheService redis)
    {
        _context = context;
        _currentUser = currentUser;
        _redis = redis;
    }

    public async Task Handle(
        UnacceptAnswerCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId!;

        var answer = await _context.Answers
            .Include(x => x.User)
            .Include(x => x.Question)
            .FirstOrDefaultAsync(
                x => x.Id == request.AnswerId,
                cancellationToken);

        if (answer == null)
            throw new KeyNotFoundException("Answer not found");

        if (answer.Question.UserId != userId)
            throw new UnauthorizedAccessException(
                "Only question owner can unaccept answer");

        if (!answer.IsAccepted)
            throw new InvalidOperationException(
                "Answer is not accepted");

        answer.IsAccepted = false;

        answer.Question.AcceptedAnswerId = null;

        answer.User.Reputation = Math.Max(0, answer.User.Reputation - 15);

        await _context.SaveChangesAsync(cancellationToken);

        await _redis.RemoveAsync("questions_all");
    }
}