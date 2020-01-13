using Microsoft.Extensions.Options;
using NotificationService.Configs;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationService.Aggregates.MailAggregate
{
    public class SmtpClientProxy: ISmtpClientProxy, IDisposable
    {
        private readonly SmtpClient _client;

        public SmtpClientProxy(IOptions<MailConfig> config)
        {
            _client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = config.Value.EnableSsl,
                Host = config.Value.Host,
                Port = config.Value.Port,
                Credentials = new NetworkCredential(config.Value.From, config.Value.Password)
            };
        }


        public async Task SendMailAsync(MailMessage msg)
        {
            await _client.SendMailAsync(msg);

            Dispose();
        }

        #region Dispose
        public void Dispose()
        {
            _client.Dispose();
        }
        #endregion
    }
}
