using Models.DTOs;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EntitiesObserver.Helpers
{
    internal class Mailer
    {
        public async static Task SendMessage(EmailDto email)
        {
            SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = email.EnableSsl,
                Host = email.Host,
                Port = email.Port,
                Credentials = new NetworkCredential(email.From, email.Password)
            };

            MailMessage message = new MailMessage
            {
                From = new MailAddress(email.From),
                Subject = email.Subject,
                Body = email.Body,
                IsBodyHtml = true
            };
            message.To.Add(email.To);

            await client.SendMailAsync(message);
        }
    }
}
