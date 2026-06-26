using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionHandler
    : IRequestHandler<UpdateQuestionRequest>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IRedisCacheService _redis;

    public UpdateQuestionHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IRedisCacheService redis)
    {
        _context = context;
        _currentUser = currentUser;
        _redis = redis;
    }

    public async Task Handle(UpdateQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (question == null)
            //throw new Exception("Question not found");
            throw new KeyNotFoundException("Question not found");

        if (question.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException("Only owner can edit this question");

        question.Title = request.Title;
        question.Description = request.Description;
        question.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _redis.RemoveAsync("questions_all");
    }
}