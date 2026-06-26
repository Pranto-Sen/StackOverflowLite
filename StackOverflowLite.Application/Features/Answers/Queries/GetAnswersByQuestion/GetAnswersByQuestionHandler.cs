using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public class GetAnswersByQuestionHandler
    : IRequestHandler<
        GetAnswersByQuestionQuery,
        List<AnswerDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAnswersByQuestionHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AnswerDto>> Handle( GetAnswersByQuestionQuery request,
        CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(
                x => x.Id == request.QuestionId,
                cancellationToken);

        if (question == null)
            throw new Exception("Question not found");

        return await _context.Answers
            .Include(x => x.User)
            .Where(x => x.QuestionId == request.QuestionId)
            .Select(x => new AnswerDto
            {
                Id = x.Id,
                Content = x.Content,
                Author = x.User.UserName!,
                CreatedAt = x.CreatedAt,
                IsAccepted = x.IsAccepted
                    //question.AcceptedAnswerId ==
                    //x.Id
            })
            .ToListAsync(cancellationToken);
    }
}