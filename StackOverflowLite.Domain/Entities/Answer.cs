using StackOverflowLite.Domain.Common;

namespace StackOverflowLite.Domain.Entities;

public class Answer : BaseEntity
{
    public string Content { get; set; } = string.Empty;

    public Guid QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public bool IsAccepted { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public ICollection<AnswerVote> Votes { get; set; }
        = new List<AnswerVote>();
}