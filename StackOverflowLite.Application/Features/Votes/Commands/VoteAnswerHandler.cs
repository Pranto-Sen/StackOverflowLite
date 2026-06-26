//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using StackOverflowLite.Application.Common.Interfaces;
//using StackOverflowLite.Domain.Entities;
//using StackOverflowLite.Domain.Enums;

//namespace StackOverflowLite.Application.Features.Votes.Commands;

//public class VoteAnswerHandler
//    : IRequestHandler<VoteAnswerCommand>
//{
//    private readonly IApplicationDbContext _context;
//    private readonly ICurrentUserService _currentUser;

//    public VoteAnswerHandler(
//        IApplicationDbContext context,
//        ICurrentUserService currentUser)
//    {
//        _context = context;
//        _currentUser = currentUser;
//    }

//    public async Task Handle(
//        VoteAnswerCommand request,
//        CancellationToken cancellationToken)
//    {
//        var userId = _currentUser.UserId!;

//        var answer = await _context.Answers
//            .Include(x => x.User)
//            .FirstOrDefaultAsync(
//                x => x.Id == request.AnswerId,
//                cancellationToken);

//        if (answer == null)
//            throw new Exception("Answer not found");

//        if (answer.UserId == userId)
//            throw new Exception("You cannot vote your own answer");

//        var existingVote =
//            await _context.AnswerVotes
//                .FirstOrDefaultAsync(
//                    x =>
//                        x.AnswerId == request.AnswerId &&
//                        x.UserId == userId,
//                    cancellationToken);

//        // First Vote
//        if (existingVote == null)
//        {
//            var vote = new AnswerVote
//            {
//                Id = Guid.NewGuid(),
//                AnswerId = request.AnswerId,
//                UserId = userId,
//                VoteType = request.VoteType
//            };

//            await _context.AnswerVotes.AddAsync(
//                vote,
//                cancellationToken);

//            if (request.VoteType == VoteType.Upvote)
//            {
//                answer.User.Reputation += 10;
//            }
//            else
//            {
//                answer.User.Reputation =
//                    Math.Max(
//                        0,
//                        answer.User.Reputation - 2);
//            }
//        }
//        else
//        {
//            // Same vote → ignore
//            if (existingVote.VoteType == request.VoteType)
//                return;

//            // Reverse old vote
//            if (existingVote.VoteType == VoteType.Upvote)
//            {
//                answer.User.Reputation =
//                    Math.Max(
//                        0,
//                        answer.User.Reputation - 10);
//            }
//            else
//            {
//                answer.User.Reputation += 2;
//            }

//            // Apply new vote
//            if (request.VoteType == VoteType.Upvote)
//            {
//                answer.User.Reputation += 10;
//            }
//            else
//            {
//                answer.User.Reputation =
//                    Math.Max(
//                        0,
//                        answer.User.Reputation - 2);
//            }

//            existingVote.VoteType = request.VoteType;
//        }

//        await _context.SaveChangesAsync(
//            cancellationToken);
//    }
//}

using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands;

public class VoteAnswerHandler
    : IRequestHandler<VoteAnswerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public VoteAnswerHandler(IApplicationDbContext context,ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(VoteAnswerCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId!;


        if (!Enum.IsDefined(typeof(VoteType), request.VoteType))
        {
            throw new FluentValidation.ValidationException(new[]
            {   new FluentValidation.Results.ValidationFailure( nameof(request.VoteType),
                "Invalid vote type. Allowed values are 1 (Upvote) or -1 (Downvote).")
            });
        }

        var answer = await _context.Answers
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == request.AnswerId,
                cancellationToken);

        if (answer == null)
            throw new KeyNotFoundException("Answer not found");

        if (answer.UserId == userId)
            throw new UnauthorizedAccessException("You cannot vote your own answer");

        var existingVote =
            await _context.AnswerVotes
                .FirstOrDefaultAsync(
                    x =>
                        x.AnswerId == request.AnswerId &&
                        x.UserId == userId,
                    cancellationToken);

        if (existingVote == null)
        {
            var vote = new AnswerVote
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AnswerId = request.AnswerId,
                VoteType = request.VoteType
            };

            await _context.AnswerVotes.AddAsync(vote, cancellationToken);

            ApplyAnswerVote(answer.User, request.VoteType);
        }
        else
        {
            if (existingVote.VoteType == request.VoteType)
                return;

            ReverseAnswerVote(answer.User, existingVote.VoteType);

            ApplyAnswerVote(answer.User, request.VoteType);

            existingVote.VoteType = request.VoteType;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void ApplyAnswerVote(Domain.Entities.ApplicationUser user, VoteType voteType)
    {
        if (voteType == VoteType.Upvote)
            user.Reputation += 10;
        else
            user.Reputation = Math.Max(0, user.Reputation - 2);
    }

    private static void ReverseAnswerVote(Domain.Entities.ApplicationUser user,VoteType voteType)
    {
        if (voteType == VoteType.Upvote)
            user.Reputation = Math.Max(0, user.Reputation - 10);
        else
            user.Reputation += 2;
    }
}