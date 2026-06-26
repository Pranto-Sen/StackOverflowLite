using MediatR;
using StackOverflowLite.Application.Features.Answers.DTOs;

namespace StackOverflowLite.Application.Features.Answers.Queries.GetAnswersByQuestion;

public record GetAnswersByQuestionQuery(Guid QuestionId): IRequest<List<AnswerDto>>;