using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.DependencyInjection;
using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;
using MailKit.Security;

namespace Iquality.Shared.OutboxMailer.Core.Mailer
{
    public class SmtpEmailSender
    {
        private ILogger _logger;

        public SmtpEmailSender(ILogger _logger)
        {
            this._logger = _logger;
        }

        //private ILogger logger = DependencyResolver.Services.GetService<ILogger>();
        public bool Send(string to, string from, string subject, string body)
        {            
            try
            {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(from?.Substring(0, to.IndexOf("@", StringComparison.Ordinal)), from));
            message.To.Add(new MailboxAddress(to?.Substring(0, to.IndexOf("@", StringComparison.Ordinal)), to));
            message.Subject = subject;            
            message.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("iprint", 25, SecureSocketOptions.None);
                    client.Send(message);
                    client.Disconnect(true);
                    _logger.LogInformation($"Email was sent to user: {to}");
                }
            return true;
            }
            catch (Exception ex)
            {                
                _logger.LogError("Smtp Send Error", ex);
                throw;
            }
        }
    }
}
