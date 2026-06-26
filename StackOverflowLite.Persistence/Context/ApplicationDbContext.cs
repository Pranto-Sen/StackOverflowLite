using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Domain.Entities;
using StackOverflowLite.Application.Common.Interfaces;

namespace StackOverflowLite.Persistence.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {

    }

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<Answer> Answers => Set<Answer>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<QuestionTag> QuestionTags => Set<QuestionTag>();

    public DbSet<QuestionVote> QuestionVotes => Set<QuestionVote>();

    public DbSet<AnswerVote> AnswerVotes => Set<AnswerVote>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly);
    }
}