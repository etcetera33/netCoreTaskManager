using System.Threading.Tasks;
using Contracts;
using MassTransit;
using NotificationService.Aggregates.HtmlAggregate;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Exceptions;

namespace NotificationService.Handlers
{
    public class EmailSendHandler : IConsumer<WorkItemAssigneeNotify>
    {
        private readonly IMailer _mailer;
        private readonly IHtmlBuilder _htmlBuilder;

        public EmailSendHandler(IMailer mailer, IHtmlBuilder htmlBuilder)
        {
            _mailer = mailer;
            _htmlBuilder = htmlBuilder;
        }

        public async Task Consume(ConsumeContext<WorkItemAssigneeNotify> context)
        {
            if (context.Message.To == null)
            {
                throw new RecipientDataNotProvidedException("No email address provided");
            }

            if (context.Message.RecieverFullName == null)
            {
                throw new RecipientDataNotProvidedException("Fullname is not provided");
            }

            if (context.Message.WorkItemId == 0)
            {
                throw new DataNotProvidedException("Work item Id is not provided");
            }

            var body = _htmlBuilder.GetEmailBodyForNewAssignee(context.Message.RecieverFullName, context.Message.WorkItemId);
            
            await _mailer.SendMessageAsync(context.Message.To, body, "New Work Item");
        }
    }
}
