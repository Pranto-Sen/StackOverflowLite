using FluentValidation;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Net;
using System.Text.Json;

namespace StackOverflowLite.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationException:

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                response.StatusCode = 400;

                response.Message = "Validation Failed";

                response.Errors = validationException.Errors
                    .Select(x => new ValidationError
                    {
                        Property = x.PropertyName,
                        Error = x.ErrorMessage
                    }).ToList();

                break;

            case UnauthorizedAccessException:

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                response.StatusCode = 401;

                response.Message = exception.Message;

                break;

            case KeyNotFoundException:

                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                response.StatusCode = 404;

                response.Message = exception.Message;

                break;

            default:

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                response.StatusCode = 500;

                response.Message = exception.Message;

                break;
        }

        var json = JsonSerializer.Serialize(response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        await context.Response.WriteAsync(json);
    }
}