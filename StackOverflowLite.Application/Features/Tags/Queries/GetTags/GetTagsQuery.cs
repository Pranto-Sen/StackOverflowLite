using MediatR;
using StackOverflowLite.Application.Features.Tags.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetTags;

public record GetTagsQuery(): IRequest<List<TagDto>>;