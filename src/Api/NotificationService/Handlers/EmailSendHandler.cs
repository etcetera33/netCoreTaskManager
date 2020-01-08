using System.Threading.Tasks;
using Contracts;
using MassTransit;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Exceptions;

namespace NotificationService.Handlers
{
    public class EmailSendHandler : IConsumer<EmailSend>
    {
        private readonly IMailer _mailer;

        public EmailSendHandler(IMailer mailer)
        {
            _mailer = mailer;
        }

        public async Task Consume(ConsumeContext<EmailSend> context)
        {
            if (context.Message.To == null)
            {
                throw new RecipientDataNotProvidedException("No email address provided");
            }

            if (context.Message.Body == null)
            {
                throw new RecipientDataNotProvidedException("Message body is not provided");
            }

            if (context.Message.Subject == null)
            {
                throw new DataNotProvidedException("Message subject is not provided");
            }

            await _mailer.SendMessageAsync(context.Message.To, context.Message.Body, context.Message.Subject);
        }
    }
}
