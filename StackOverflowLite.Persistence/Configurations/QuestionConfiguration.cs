using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Persistence.Configurations;

public class QuestionConfiguration
    : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Description)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Answers)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId);

        builder.HasMany(x => x.Votes)
            .WithOne(x => x.Question)
            .HasForeignKey(x => x.QuestionId);

        builder.HasOne(x => x.AcceptedAnswer)
            .WithMany()
            .HasForeignKey(x => x.AcceptedAnswerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}