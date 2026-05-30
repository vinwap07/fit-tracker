using System.ComponentModel.DataAnnotations;
using System.Net;
using Application.Exceptions;
using Domain.Exceptions;

namespace Presentation.Middlewares;

public record ErrorResponse(
    int StatusCode,
    string Message);

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                logger.LogWarning("Ошибка произошла ПОСЛЕ начала отправки ответа. Middleware бессилен.");
                throw; 
            }
            
            var (statusCode, message) = ex switch
            {
                BadRequestException e => (HttpStatusCode.BadRequest, e.Message),
                NotFoundException e => (HttpStatusCode.NotFound, e.Message),
                ForbiddenAccessException e => (HttpStatusCode.Forbidden, e.Message),
                FluentValidation.ValidationException e => (HttpStatusCode.BadRequest,
                    string.Join("; ", e.Errors.Select(er => er.ErrorMessage))),
                ValidationException e => (HttpStatusCode.BadRequest,
                    e.Message ?? "Validation failed"),
                DomainException e => (HttpStatusCode.BadRequest, e.Message),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                logger.LogError(ex, "Unhandled Exception: {Message}", message);
            }
            else
            {
                logger.LogWarning("Business Exception: {Message}", message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse(context.Response.StatusCode, message);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
