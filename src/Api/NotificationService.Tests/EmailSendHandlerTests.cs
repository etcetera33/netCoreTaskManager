using Contracts;
using Core.Adapters;
using MassTransit;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Handlers;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NotificationService.Tests
{
    public class EmailSendHandlerTests
    {
        private readonly IMailer _mailer;
        private readonly EmailSendHandler _handler;
        private readonly ILoggerAdapter<EmailSendHandler> _logger;
        private ConsumeContext<EmailSend> _consumeContext;

        public EmailSendHandlerTests()
        {
            _mailer = Substitute.For<IMailer>();
            _consumeContext = Substitute.For<ConsumeContext<EmailSend>>();
            _logger = Substitute.For<ILoggerAdapter<EmailSendHandler>>();

            _handler = new EmailSendHandler(_mailer, _logger);
        }

        [Theory]
        [InlineData("Message body", "Message subject", "Message to")]
        public async Task Should_Successfully_Invoke_Send_Method(string body, string subject, string to)
        {
            _consumeContext.Message.Returns(new EmailSend { Body = body, Subject = subject, To = to });

            await _handler.Consume(_consumeContext);

            await _mailer.Received(1).SendMessageAsync(to, body, subject);

            _logger.Received(1).Information($"Email sent to: {to}, with body: {body}");
        }

        [Fact]
        public async Task Should_Throw_Message_ArgumentNullException()
        {
            _consumeContext.Message.Returns(value => null);

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

        [Theory]
        [InlineData("Message body", "Message subject", null)]
        public async Task Should_Throw_Message_To_ArgumentNullException(string body, string subject, string to)
        {
            _consumeContext.Message.Returns(new EmailSend { Body = body, Subject = subject, To = to });

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

        [Theory]
        [InlineData(null, "Message subject", "Message to")]
        public async Task Should_Throw_Message_Body_ArgumentNullException(string body, string subject, string to)
        {
            _consumeContext.Message.Returns(new EmailSend { Body = body, Subject = subject, To = to });

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

        [Theory]
        [InlineData("Message body", null, "Message to")]
        public async Task Should_Throw_Message_Subject_ArgumentNullException(string body, string subject, string to)
        {
            _consumeContext.Message.Returns(new EmailSend { Body = body, Subject = subject, To = to });

            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Consume(_consumeContext));
        }

    }
}
