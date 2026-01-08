using EmailNotificationApi.Interfaces;
using System.Net;
using System.Net.Mail;

namespace EmailNotificationApi.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var host = _configuration.GetValue<string>("Smtp:Host");
        var port = _configuration.GetValue<int>("Smtp:Port");
        var username = _configuration.GetValue<string>("Smtp:Username");
        var password = _configuration.GetValue<string>("Smtp:Password");

        using var client = new SmtpClient(host, port);
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential(username, password);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(username),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
    }
}
