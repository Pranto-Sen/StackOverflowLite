using MediatR;

namespace StackOverflowLite.Application.Features.Tags.DTOs;

public class CreateTagRequest : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
}