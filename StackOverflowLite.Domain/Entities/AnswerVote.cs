using StackOverflowLite.Domain.Common;
using StackOverflowLite.Domain.Enums;

namespace StackOverflowLite.Domain.Entities;

public class AnswerVote : BaseEntity
{
    public Guid AnswerId { get; set; }

    public Answer Answer { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    public VoteType VoteType { get; set; }
}