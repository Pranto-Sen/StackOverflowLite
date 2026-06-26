using MediatR;
using Microsoft.AspNetCore.Identity;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Auth.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Auth.Queries.GetProfile;

public class GetProfileHandler
    : IRequestHandler<GetProfileQuery, ProfileResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUser;

    public GetProfileHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUser)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_currentUser.UserId!);

        if (user == null)
            throw new KeyNotFoundException("User not found");

        return new ProfileResponse
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            Reputation = user.Reputation
        };
    }
}