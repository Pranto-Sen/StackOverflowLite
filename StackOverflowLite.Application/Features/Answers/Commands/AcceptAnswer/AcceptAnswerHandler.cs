using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.AcceptAnswer;

public class AcceptAnswerHandler
    : IRequestHandler<AcceptAnswerRequest>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IRedisCacheService _redis;

    public AcceptAnswerHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IRedisCacheService redis)
    {
        _context = context;
        _currentUser = currentUser;
        _redis = redis;
    }

    public async Task Handle(AcceptAnswerRequest request, CancellationToken cancellationToken)
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
            throw new UnauthorizedAccessException("Only question owner can accept answer");

        if (answer.UserId == userId)
            throw new InvalidOperationException("Cannot accept your own answer");

        var oldAccepted =
            await _context.Answers
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x =>
                        x.QuestionId ==
                        answer.QuestionId &&
                        x.IsAccepted,
                    cancellationToken);

        //if (oldAccepted != null)
        //{
        //    oldAccepted.IsAccepted = false;

        //    oldAccepted.User.Reputation =
        //        Math.Max(
        //            0,
        //            oldAccepted.User.Reputation - 15);
        //}

        //answer.IsAccepted = true;

        //answer.User.Reputation += 15;

        if (oldAccepted != null)
        {
            oldAccepted.IsAccepted = false;

            oldAccepted.User.Reputation = Math.Max(0, oldAccepted.User.Reputation - 15);
        }

        answer.IsAccepted = true;

        answer.Question.AcceptedAnswerId = answer.Id;

        answer.User.Reputation += 15;

        await _context.SaveChangesAsync(cancellationToken);

        await _redis.RemoveAsync("questions_all");
    }
}