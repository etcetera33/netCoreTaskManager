using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NotificationService.Configs;

namespace NotificationService.Aggregates.MailAggregate
{
    public class Mailer : IMailer
    {
        private readonly IOptions<MailConfig> _mailConfig;

        public Mailer(IOptions<MailConfig> mailConfig)
        {
            _mailConfig = mailConfig;
        }

        public async Task SendMessageAsync(string to, string body, string subject)
        {
            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _mailConfig.Value.EnableSsl,
                Host = _mailConfig.Value.Host,
                Port = _mailConfig.Value.Port,
                Credentials = new NetworkCredential(_mailConfig.Value.From, _mailConfig.Value.Password)
            };

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_mailConfig.Value.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
}
