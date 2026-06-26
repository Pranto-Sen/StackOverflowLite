using MediatR;

namespace StackOverflowLite.Application.Features.Auth.DTOs;

public class RegisterRequest : IRequest<AuthResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}