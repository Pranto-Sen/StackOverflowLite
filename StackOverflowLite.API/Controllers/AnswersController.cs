using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;
using StackOverflowLite.Application.Features.Answers.Commands.UnacceptAnswer;
using StackOverflowLite.Application.Features.Answers.DTOs;
using StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;
using StackOverflowLite.Domain.Entities;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswersController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnswersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateAnswerRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("question/{questionId}")]
    public async Task<IActionResult> GetByQuestion(Guid questionId)
    {
        return Ok(await _mediator.Send(new GetAnswersByQuestionQuery(questionId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateAnswerRequest request)
    {
        request.Id = id;

        await _mediator.Send(request);

        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAnswerCommand(id));

        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("accept")]
    public async Task<IActionResult> Accept(AcceptAnswerRequest request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("unaccept/{answerId}")]
    public async Task<IActionResult> Unaccept(Guid answerId)
    {
        await _mediator.Send(new UnacceptAnswerCommand(answerId));

        return NoContent();
    }
}