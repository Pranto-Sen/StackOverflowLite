namespace StackOverflowLite.Application.Features.Questions.DTOs;

public class QuestionDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public long ViewCount { get; set; }
    public Guid? AcceptedAnswerId { get; set; }

    public DateTime CreatedAt { get; set; }
}