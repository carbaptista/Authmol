using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Authmol.Application.Services.Email;
public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    public MailService(IOptions<MailSettings> mailSettingsOptions, IConfiguration configuration)
    {
        _mailSettings = mailSettingsOptions.Value;
        //var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //if (env == "Development")
        //{
        //    _mailSettings.Password = configuration["MailSettings:Password"] ??= "";
        //}
    }

    public async Task<bool> SendVerificationMailAsync(MailData mailData)
    {
        try
        {
            using MimeMessage emailMessage = new();

            MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);

            MailboxAddress emailTo = new(mailData.EmailToName, mailData.EmailToId);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = mailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new();
            emailBodyBuilder.HtmlBody = mailData.EmailBody;

            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            using SmtpClient mailClient = new();

            await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
