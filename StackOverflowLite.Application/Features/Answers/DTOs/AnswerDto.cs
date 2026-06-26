namespace StackOverflowLite.Application.Features.Answers.DTOs;

public class AnswerDto
{
    public Guid Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public bool IsAccepted { get; set; }

    public DateTime CreatedAt { get; set; }
}