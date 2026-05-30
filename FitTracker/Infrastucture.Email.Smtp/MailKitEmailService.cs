using Application.Abstractions;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace Infrastucture.EmailService;

public class MailKitEmailService(IOptions<SmtpOptions> options) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(options.Value.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        // TODO: заменить простой текст на формирование html
        email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(options.Value.Host, options.Value.Port, SecureSocketOptions.Auto);
        await smtp.AuthenticateAsync(options.Value.UserName, options.Value.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}