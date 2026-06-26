using StackOverflowLite.Domain.Common;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Domain.Entities;

public class QuestionVote : BaseEntity
{
    public Guid QuestionId { get; set; }

    public Question Question { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public VoteType VoteType { get; set; }
}