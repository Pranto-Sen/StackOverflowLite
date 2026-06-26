using MediatR;
using StackOverflowLite.Application.Features.Auth.DTOs;

namespace StackOverflowLite.Application.Features.Auth.Queries.GetProfile;

public record GetProfileQuery(): IRequest<ProfileResponse>;