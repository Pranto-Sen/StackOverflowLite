using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackOverflowLite.Application.Features.Votes.Commands;

namespace StackOverflowLite.API.Controllers;

[ApiController]
[Route("api/votes")]
public class VotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("question")]
    public async Task<IActionResult> VoteQuestion(VoteQuestionCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("answer")]
    public async Task<IActionResult> VoteAnswer(VoteAnswerCommand request)
    {
        await _mediator.Send(request);

        return NoContent();
    }
}