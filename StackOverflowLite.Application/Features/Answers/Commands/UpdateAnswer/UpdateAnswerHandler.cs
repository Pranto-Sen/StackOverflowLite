using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Commands.UpdateAnswer;

public class UpdateAnswerHandler
    : IRequestHandler<UpdateAnswerRequest>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateAnswerHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        UpdateAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var answer = await _context.Answers
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (answer == null)
            throw new KeyNotFoundException("Answer not found");

        if (answer.UserId != _currentUser.UserId)
        {
            throw new UnauthorizedAccessException("Only owner can edit answer");
        }

        answer.Content = request.Content;
        answer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}