using Microsoft.Extensions.Options;
using NotificationService.Aggregates.MailAggregate;
using NotificationService.Configs;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NotificationService.Tests
{
    public class MailerTests
    {
        private readonly IMailer _mailer;

        public MailerTests()
        {
            var mailOptions = Options.Create(new MailConfig());

            mailOptions.Value.From = "dataarttest@gmail.com";

            var smtp = Substitute.For<ISmtpClientProxy, IDisposable>();
            _mailer = Substitute.For<Mailer>(mailOptions, smtp);
        }

        [Theory]
        [InlineData("EmailExample@gmail.com", "Body", "Subject")]
        public async Task SendMessageAsync_Should_Successfully_Send_Message(string to, string body, string subject)
        {
            await _mailer.SendMessageAsync(to, body, subject);
            
            Assert.True(true);
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
