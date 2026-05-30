using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture.EmailService;

public static class SmtpExtensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configurations)
    {
        services.Configure<SmtpOptions>(configurations.GetSection(SmtpOptions.SectionName));
        services.AddTransient<IEmailService, MailKitEmailService>();
        
        return services;
    }
}