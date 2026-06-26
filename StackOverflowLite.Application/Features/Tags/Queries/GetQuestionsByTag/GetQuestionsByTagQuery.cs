using MediatR;
using StackOverflowLite.Application.Features.Questions.DTOs;

namespace StackOverflowLite.Application.Features.Tags.Queries.GetQuestionsByTag;

public record GetQuestionsByTagQuery(string TagName): IRequest<List<QuestionDto>>;