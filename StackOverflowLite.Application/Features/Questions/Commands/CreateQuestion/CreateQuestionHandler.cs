using MediatR;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionHandler: IRequestHandler<CreateQuestionRequest, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IRedisCacheService _redis;


    public CreateQuestionHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IRedisCacheService redis)
    {
        _context = context;
        _currentUser = currentUser;
        _redis = redis;
    }

    public async Task<Guid> Handle(
        CreateQuestionRequest request,
        CancellationToken cancellationToken)
    {
        var question = new Question
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            UserId = _currentUser.UserId!
        };

        _context.Questions.Add(question);

        await _context.SaveChangesAsync(cancellationToken);

        await _redis.RemoveAsync("questions_all");

        return question.Id;
    }
}