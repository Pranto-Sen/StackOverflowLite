namespace StackOverflowLite.Application.Features.Auth.DTOs;

public class ProfileResponse
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int Reputation { get; set; }
}