using MediatR;
using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Auth.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Auth.Commands.Register;

public class RegisterHandler
    : IRequestHandler<RegisterRequest, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public RegisterHandler(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(", ",
                result.Errors.Select(x => x.Description)));
        }

        var token = _jwtService.GenerateToken(
            user.Id,
            user.Email!,
            user.UserName!);

        return new AuthResponse
        {
            Token = token,
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!
        };
    }
}