using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Question> Questions { get; }

    DbSet<Answer> Answers { get; }

    DbSet<Tag> Tags { get; }

    DbSet<QuestionTag> QuestionTags { get; }

    DbSet<QuestionVote> QuestionVotes { get; }

    DbSet<AnswerVote> AnswerVotes { get; }

    DbSet<ApplicationUser> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}