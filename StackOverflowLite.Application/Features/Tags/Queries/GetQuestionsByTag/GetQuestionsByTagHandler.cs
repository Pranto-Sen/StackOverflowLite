using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;

public class GetQuestionsByTagHandler: IRequestHandler<GetQuestionsByTagQuery, List<QuestionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetQuestionsByTagHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionDto>> Handle(
        GetQuestionsByTagQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Include(x => x.User)
            .Include(x => x.QuestionTags)
            .ThenInclude(x => x.Tag)
            .Where(x =>
                x.QuestionTags.Any(t =>
                    t.Tag.Name.ToLower() ==
                    request.TagName.ToLower()))
            .Select(x => new QuestionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Author = x.User.UserName!,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}