using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Application.Features.Votes.Commands;

public class VoteQuestionHandler: IRequestHandler<VoteQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public VoteQuestionHandler(IApplicationDbContext context,ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(VoteQuestionCommand request,CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId!;

        if (!Enum.IsDefined(typeof(VoteType), request.VoteType))
        {
            throw new FluentValidation.ValidationException(new[]
            {   new FluentValidation.Results.ValidationFailure( nameof(request.VoteType),
                "Invalid vote type. Allowed values are 1 (Upvote) or -1 (Downvote).")
            });
        }

        var question = await _context.Questions
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == request.QuestionId,
                cancellationToken);

        if (question == null)
            //throw new Exception("Question not found");
            throw new KeyNotFoundException("Question not found");

        if (question.UserId == userId)
            throw new UnauthorizedAccessException("You cannot vote your own question");

        var existingVote = await _context.QuestionVotes
                .FirstOrDefaultAsync(
                    x =>
                        x.QuestionId == request.QuestionId &&
                        x.UserId == userId,
                    cancellationToken);

        if (existingVote == null)
        {
            var vote = new QuestionVote
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                QuestionId = request.QuestionId,
                VoteType = request.VoteType
            };

            await _context.QuestionVotes.AddAsync(vote, cancellationToken);

            ApplyQuestionVote(question.User, request.VoteType);
        }
        else
        {
            if (existingVote.VoteType == request.VoteType)
                return;

            ReverseQuestionVote(question.User, existingVote.VoteType);

            ApplyQuestionVote(question.User, request.VoteType);

            existingVote.VoteType = request.VoteType;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static void ApplyQuestionVote(Domain.Entities.ApplicationUser user, VoteType voteType)
    {
        if (voteType == VoteType.Upvote)
            user.Reputation += 5;
        else
            user.Reputation = Math.Max(0, user.Reputation - 1);
    }

    private static void ReverseQuestionVote(Domain.Entities.ApplicationUser user, VoteType voteType)
    {
        if (voteType == VoteType.Upvote)
            user.Reputation = Math.Max(0, user.Reputation - 5);
        else
            user.Reputation += 1;
    }
}