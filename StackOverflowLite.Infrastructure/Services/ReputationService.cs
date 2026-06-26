using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Infrastructure.Services;

public class ReputationService : IReputationService
{
    private readonly IApplicationDbContext _context;

    public ReputationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddQuestionUpvoteAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstAsync(
                x => x.Id == userId,
                cancellationToken);

        user.Reputation += 5;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddQuestionDownvoteAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstAsync(
                x => x.Id == userId,
                cancellationToken);

        user.Reputation = Math.Max(0, user.Reputation - 1);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAnswerUpvoteAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstAsync(
                x => x.Id == userId,
                cancellationToken);

        user.Reputation += 10;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAnswerDownvoteAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstAsync(
                x => x.Id == userId,
                cancellationToken);

        user.Reputation = Math.Max(0, user.Reputation - 2);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAcceptedAnswerAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstAsync(
                x => x.Id == userId,
                cancellationToken);

        user.Reputation += 15;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAcceptedAnswerAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstAsync(
                x => x.Id == userId,
                cancellationToken);

        user.Reputation = Math.Max(0, user.Reputation - 15);

        await _context.SaveChangesAsync(cancellationToken);
    }
}