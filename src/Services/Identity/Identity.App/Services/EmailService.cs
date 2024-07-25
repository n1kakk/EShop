using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Identity.App.Configs;

namespace Identity.App.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<SmtpConfig> smtpConfig;

    public EmailService(IOptions<SmtpConfig> smtpConfig)
    {
        this.smtpConfig = smtpConfig;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MailMessage
        {
            From = new MailAddress(smtpConfig.Value.SenderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        using (var emailClient = new SmtpClient(smtpConfig.Value.Host, smtpConfig.Value.Port))
        {
            emailClient.Credentials = new NetworkCredential(
                smtpConfig.Value.User,
                smtpConfig.Value.Password
                );
            emailClient.EnableSsl = true;
            await emailClient.SendMailAsync(message);
        };
    }
}
