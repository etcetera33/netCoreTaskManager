using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationService.Aggregates.MailAggregate
{
    public interface ISmtpClientProxy
    {
        Task SendMailAsync(MailMessage msg);
    }
}
