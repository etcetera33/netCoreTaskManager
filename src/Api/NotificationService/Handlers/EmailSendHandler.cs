using System;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using NotificationService.Aggregates.MailAggregate;
using Core.Adapters;

namespace NotificationService.Handlers
{
    public class EmailSendHandler : IConsumer<EmailSend>
    {
        private readonly IMailer _mailer;
        private readonly ILoggerAdapter<EmailSendHandler> _logger;

        public EmailSendHandler(IMailer mailer, ILoggerAdapter<EmailSendHandler> logger)
        {
            _mailer = mailer;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<EmailSend> context)
        {
            try
            {
                if (context.Message == null)
                {
                    throw new ArgumentNullException("Message is null");
                }

                if (context.Message.To == null)
                {
                    throw new ArgumentNullException("No email address provided");
                }

                if (context.Message.Body == null)
                {
                    throw new ArgumentNullException("Message body is not provided");
                }

                if (context.Message.Subject == null)
                {
                    throw new ArgumentNullException("Message subject is not provided");
                }

                await _mailer.SendMessageAsync(context.Message.To, context.Message.Body, context.Message.Subject);

                _logger.Information($"Email sent to: {context.Message.To}, with body: {context.Message.Body}");
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message);
                return;
            }
        }
    }
}
