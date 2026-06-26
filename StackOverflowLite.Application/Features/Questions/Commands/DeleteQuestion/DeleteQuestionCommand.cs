using MediatR;

namespace StackOverflowLite.Application.Features.Questions.Commands.DeleteQuestion;

public record DeleteQuestionCommand(Guid Id): IRequest;