using MediatR;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Application.Common.Interfaces;
using StackOverflowLite.Application.Features.Tags.DTOs;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.Application.Features.Tags.Commands.CreateTag;

public class CreateTagHandler: IRequestHandler<CreateTagRequest, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateTagHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTagRequest request,CancellationToken cancellationToken)
    {
        var exists = await _context.Tags
            .AnyAsync(
                x => x.Name.ToLower() ==
                     request.Name.ToLower(),
                cancellationToken);

        if (exists)
            throw new Exception("Tag already exists");

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim()
        };

        _context.Tags.Add(tag);

        await _context.SaveChangesAsync(cancellationToken);

        return tag.Id;
    }
}