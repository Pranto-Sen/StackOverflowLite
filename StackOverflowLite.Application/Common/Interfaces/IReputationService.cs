namespace StackOverflowLite.Application.Common.Interfaces;

public interface IReputationService
{
    Task AddQuestionUpvoteAsync(string userId, CancellationToken cancellationToken);

    Task AddQuestionDownvoteAsync(string userId, CancellationToken cancellationToken);

    Task AddAnswerUpvoteAsync(string userId, CancellationToken cancellationToken);

    Task AddAnswerDownvoteAsync(string userId, CancellationToken cancellationToken);

    Task AddAcceptedAnswerAsync(string userId, CancellationToken cancellationToken);

    Task RemoveAcceptedAnswerAsync(string userId, CancellationToken cancellationToken);
}