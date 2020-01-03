using Models.DTOs;
using System.Threading.Tasks;

namespace NotificationService.Aggregates.MailAggregate
{
    public interface IMailer
    {
        Task SendMessageAsync(string to, string body, string subject);
    }
}
