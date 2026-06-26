using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestions;

public class GetQuestionsHandler
    : IRequestHandler<GetQuestionsQuery, List<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IRedisCacheService _redis;

    private const string CacheKey = "questions_all";

    public GetQuestionsHandler(
        IApplicationDbContext context,
        IRedisCacheService redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<List<QuestionDto>> Handle(
        GetQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        
        var cachedQuestions = await _redis.GetAsync<List<QuestionDto>>(CacheKey);

        if (cachedQuestions != null)
        {
            return cachedQuestions;
        }

        //await Task.Delay(10000);
        
        var questions = await _context.Questions
            .AsNoTracking()
            .Include(x => x.User)
            .Select(x => new QuestionDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Author = x.User.UserName!,
                AcceptedAnswerId = x.AcceptedAnswerId,
                CreatedAt = x.CreatedAt,
                ViewCount = x.ViewCount
            })
            .ToListAsync(cancellationToken);

        //  Save Into Redis (5 Minutes)
        await _redis.SetAsync(CacheKey,questions,TimeSpan.FromMinutes(5));

        return questions;
    }
}