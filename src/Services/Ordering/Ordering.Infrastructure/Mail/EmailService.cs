using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastucture;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {

        public EmailSettings _emailSettings;

        public ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;

        }

        public async Task<bool> SendEmail(Email email)
        {
            //使用SendGrid發送email的服務
            var client = new SendGridClient(_emailSettings.ApiKey);

            //寄件人
            var from = new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName);
            //主旨
            var subject = email.Subject;
            //收件人
            var to = new EmailAddress(email.To);

            //email本體
            var emailBody = email.Body;
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";

            //建立信件
            var msg = MailHelper.CreateSingleEmail(from, to, subject, emailBody, htmlContent);

            //發送信件
            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation("Email已寄出");

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            _logger.LogError("Email發送失敗");

            return false;
        }
    }
}
