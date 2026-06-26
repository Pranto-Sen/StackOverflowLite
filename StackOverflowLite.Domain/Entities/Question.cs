using StackOverflowLite.Domain.Common;

namespace StackOverflowLite.Domain.Entities;

public class Question : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public long ViewCount { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    // Accepted Answer

    public Guid? AcceptedAnswerId { get; set; }

    public Answer? AcceptedAnswer { get; set; }

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public ICollection<QuestionTag> QuestionTags { get; set; }  = new List<QuestionTag>();

    public ICollection<QuestionVote> Votes { get; set; } = new List<QuestionVote>();
}