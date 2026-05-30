using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        
        logger.LogInformation("Handling {Name}. Data: {@Request}", requestName, request);

        var response = await next();
        
        logger.LogInformation("Handled {Name}. Result: {@Response}", requestName, response);

        return response;
    }
}