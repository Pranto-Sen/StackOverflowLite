using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Questions.Queries.GetQuestionById;

public class GetQuestionByIdHandler
    : IRequestHandler<GetQuestionByIdQuery,
        QuestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IRedisCacheService _redis;

    public GetQuestionByIdHandler(
        IApplicationDbContext context, IRedisCacheService redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<QuestionDto> Handle(
        GetQuestionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (question == null)
            //throw new Exception("Question not found");
            throw new KeyNotFoundException("Question not found");


       
        var redisKey = $"question_views_{question.Id}";

       
        var totalViews = await _redis.IncrementAsync(redisKey);

        
        question.ViewCount = totalViews;

        await _context.SaveChangesAsync(cancellationToken);

        await _redis.RemoveAsync("questions_all");

        //return new QuestionDto
        //{
        //    Id = question.Id,
        //    Title = question.Title,
        //    Description = question.Description,
        //    Author = question.User.UserName!,
        //    CreatedAt = question.CreatedAt
        //};
        return new QuestionDto
        {
            Id = question.Id,
            Title = question.Title,
            Description = question.Description,
            Author = question.User.UserName!,
            AcceptedAnswerId = question.AcceptedAnswerId,
            CreatedAt = question.CreatedAt,
            ViewCount = totalViews
        };
    }
}