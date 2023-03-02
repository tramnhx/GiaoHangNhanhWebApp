using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace BHSNetCoreLib.EmailUtil.SendEmail
{
    public class SendEmailService : ISendEmailService
    {
        private readonly EmailSettings emailSettings;

        private readonly ILogger<SendEmailService> logger;


        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public SendEmailService(IOptions<EmailSettings> _emailSettings, ILogger<SendEmailService> _logger)
        {
            emailSettings = _emailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendEmailService");
        }

        // Gửi email, theo nội dung trong emailContent
        public async Task SendEmail(EmailContent emailContent)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSettings.Email);
            email.To.Add(MailboxAddress.Parse(emailContent.To));
            email.Subject = emailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = emailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSettings.Email, emailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                logger.LogInformation("Lỗi gửi mail");
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + emailContent.To);

        }
    }
}
