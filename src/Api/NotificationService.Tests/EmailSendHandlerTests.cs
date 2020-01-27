using Contracts;
using Core.Adapters;
using MassTransit;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Handlers;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace NotificationService.Tests
{
    public class EmailSendHandlerTests
    {
        private readonly IMailer _mailer;
        private readonly EmailSendHandler _handler;
        private readonly ILoggerAdapter<EmailSendHandler> _logger;
        private readonly ConsumeContext<EmailSend> _consumeContext;

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
        }

        [Fact]
        public async Task Should_Not_Recieve_Mailer_Email_Null()
        {
            _consumeContext.Message.Returns(value => null);
            await _handler.Consume(_consumeContext);

            await _mailer.DidNotReceive().SendMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineData(null, "Message subject", "Message to")]
        [InlineData("Message body", null, "Message to")]
        [InlineData("Message body", "Message subject", null)]
        public async Task Should_Not_Recieve_Mailer_Properties_Null(string body, string subject, string to)
        {
            _consumeContext.Message.Returns(new EmailSend { Body = body, Subject = subject, To = to });
            await _handler.Consume(_consumeContext);

            await _mailer.DidNotReceive().SendMessageAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
