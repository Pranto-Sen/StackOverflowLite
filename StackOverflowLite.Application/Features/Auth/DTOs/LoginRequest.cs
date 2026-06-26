using MediatR;

namespace StackOverflowLite.Application.Features.Auth.DTOs;

public class LoginRequest : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}