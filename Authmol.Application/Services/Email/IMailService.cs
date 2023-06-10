namespace Authmol.Application.Services.Email;
public interface IMailService
{
    Task<bool> SendVerificationMailAsync(MailData mailData);
}
