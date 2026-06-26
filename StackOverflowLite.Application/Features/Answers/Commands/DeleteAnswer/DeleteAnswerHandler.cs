using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;

public class DeleteAnswerHandler
    : IRequestHandler<DeleteAnswerCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteAnswerHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeleteAnswerCommand request,
        CancellationToken cancellationToken)
    {
        var answer = await _context.Answers
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (answer == null)
            throw new KeyNotFoundException(
                "Answer not found");

        if (answer.UserId !=
            _currentUser.UserId)
        {
            throw new UnauthorizedAccessException(
                "Only owner can delete answer");
        }

        //if (answer.IsAccepted)
        //{
        //    throw new InvalidOperationException(
        //        "Accepted answer must be unaccepted first");
        //}

        var question = await _context.Questions
            .FirstOrDefaultAsync(
                x => x.AcceptedAnswerId ==
                     answer.Id,
                cancellationToken);

        if (question != null)
        {
            throw new InvalidOperationException(
                "Cannot delete accepted answer. Unaccept it first.");
        }

        _context.Answers.Remove(answer);

        await _context.SaveChangesAsync(cancellationToken);
    }
}