using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Tags.Commands.AssignTagsToQuestion;

public class AssignTagsToQuestionHandler
    : IRequestHandler<AssignTagsToQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AssignTagsToQuestionHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(AssignTagsToQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(x => x.QuestionTags)
            .FirstOrDefaultAsync(
                x => x.Id == request.QuestionId,
                cancellationToken);

        if (question == null)
            throw new Exception("Question not found");

        if (question.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException("Unauthorized");

        _context.QuestionTags.RemoveRange(question.QuestionTags);

        foreach (var tagId in request.TagIds)
        {
            _context.QuestionTags.Add(
                new QuestionTag
                {
                    QuestionId = request.QuestionId,
                    TagId = tagId
                });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}