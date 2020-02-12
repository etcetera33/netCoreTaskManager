using Microsoft.Extensions.Options;
using NotificationService.Configs;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationService.Aggregates.MailAggregate
{
    public class Mailer : IMailer
    {
        private readonly IOptions<MailConfig> _mailConfig;
        private readonly ISmtpClientProxy _smtpClient;

        public Mailer(IOptions<MailConfig> mailConfig, ISmtpClientProxy smtpClient)
        {
            _mailConfig = mailConfig;
            _smtpClient = smtpClient;
        }

        public async Task SendMessageAsync(string to, string body, string subject)
        {
            Validate(to, body, subject);

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_mailConfig.Value.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        private void Validate(string to, string body, string subject)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("Reciever not provided");
            }
            else
            {
                // throws Format exception in case of wrong email format
                new MailAddress(to);
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentException("Body not provided");
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("Subject not provided");
            }
        }
    }
}
