using MediatR;

namespace StackOverflowLite.Application.Features.Answers.Commands.DeleteAnswer;

public record DeleteAnswerCommand(Guid Id): IRequest;