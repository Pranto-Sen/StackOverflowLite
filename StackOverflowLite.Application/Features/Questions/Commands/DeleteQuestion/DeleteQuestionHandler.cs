using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionHandler
    : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IRedisCacheService _redis;

    public DeleteQuestionHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IRedisCacheService redis)
    {
        _context = context;
        _currentUser = currentUser;
        _redis = redis;
    }

    public async Task Handle(DeleteQuestionCommand request,CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (question == null)
            //throw new Exception("Question not found");
            throw new KeyNotFoundException("Question not found");

        if (question.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException("Only owner can delete question");

        _context.Questions.Remove(question);

        await _context.SaveChangesAsync(cancellationToken);

        await _redis.RemoveAsync("questions_all");
    }
}