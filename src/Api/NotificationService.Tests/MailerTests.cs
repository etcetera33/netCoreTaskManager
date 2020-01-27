using Microsoft.Extensions.Options;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Configs;
using NSubstitute;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

namespace NotificationService.Tests
{
    public class MailerTests
    {
        private readonly IMailer _mailer;
        private readonly ISmtpClientProxy _smtp;
        private readonly MailMessage _message;

        public MailerTests()
        {
            var from = "dataarttest@gmail.com";

            var mailOptions = Options.Create(new MailConfig());
            mailOptions.Value.From = from;

            _smtp = Substitute.For<ISmtpClientProxy, IDisposable>();

            _mailer = Substitute.For<Mailer>(mailOptions, _smtp);

            _message = new MailMessage
            {
                From = new MailAddress(from),
            };
        }

        [Theory]
        [InlineData("EmailExample@gmail.com", "Body", "Subject")]
        public async Task SendMessageAsync_Should_Receive_With_One_Message_Arg(string to, string body, string subject)
        {
            _message.Subject = subject;
            _message.Body = body;
            _message.To.Add(to);

            await _mailer.SendMessageAsync(to, body, subject);

            await _smtp.ReceivedWithAnyArgs(1).SendMailAsync(_message);
        }

        [Theory]
        [InlineData("EmailExample@gmail.com", "Body", "Subject")]
        public async Task SendMessageAsync_Should_Receive_Proper_Type_Arg(string to, string body, string subject)
        {
            _message.Subject = subject;
            _message.Body = body;
            _message.To.Add(to);

            await _mailer.SendMessageAsync(to, body, subject);

            await _smtp.Received().SendMailAsync(Arg.Any<MailMessage>());
        }

        [Theory]
        [InlineData("EmailExample@gmail.com", "Body", "Subject")]
        public async Task SendMessageAsync_Should_Receive_Same_Arg(string to, string body, string subject)
        {
            _message.Subject = subject;
            _message.Body = body;
            _message.To.Add(to);

            await _mailer.SendMessageAsync(to, body, subject);

            await _smtp.Received().SendMailAsync(Arg.Is<MailMessage>(
                message => message.Subject == _message.Subject
                    && message.Body == _message.Body
                    && message.To.AsEnumerable().SequenceEqual(_message.To.AsEnumerable())
                    && message.From.Address == _message.From.Address
                ));
        }

        [Theory]
        [InlineData("EmailExample@gmail.com", "Body", "Subject")]
        public async Task SendMessageAsync_Should_Throw_Validation_Errors(string to, string body, string subject)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _mailer.SendMessageAsync("", body, subject));
            await Assert.ThrowsAsync<ArgumentException>(() => _mailer.SendMessageAsync(to, "", subject));
            await Assert.ThrowsAsync<ArgumentException>(() => _mailer.SendMessageAsync(to, body, ""));

            await Assert.ThrowsAsync<FormatException>(() => _mailer.SendMessageAsync("EmailExample.com", body, subject));
        }
    }
}
