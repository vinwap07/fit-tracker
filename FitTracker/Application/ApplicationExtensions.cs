using System.Reflection;
using Application.Abstractions.Auth;
using Application.Behaviors;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            
        services.AddValidatorsFromAssembly(assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddAutoMapper(_ => {}, assembly);
        
        services.AddScoped<IIdentityService, IdentityService>();
        
        return services;
    }
}