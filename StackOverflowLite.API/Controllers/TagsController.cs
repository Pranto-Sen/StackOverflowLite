using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Tags.Commands.AssignTagsToQuestion;
using StackOverflowLite.Application.Features.Tags.DTOs;
using StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;
using StackOverflowLite.Application.Features.Tags.Queries.GetTags;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _mediator.Send(new GetTagsQuery()));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTagRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("assign")]
    public async Task<IActionResult> Assign(AssignTagsToQuestionCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [HttpGet("questions/{tagName}")]
    public async Task<IActionResult> GetQuestions(string tagName)
    {
        return Ok(await _mediator.Send(new GetQuestionsByTagQuery(tagName)));
    }
}