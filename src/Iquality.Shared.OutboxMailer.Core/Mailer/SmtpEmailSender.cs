using Microsoft.Extensions.Logging;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;
using MailKit.Security;
using Iquality.Shared.OutboxMailer.Core.Models;

namespace Iquality.Shared.OutboxMailer.Core.Mailer
{
    public class SmtpEmailSender
    {
        private ILogger _logger;

        public SmtpEmailSender(ILogger _logger)
        {
            this._logger = _logger;
        }

        public bool Send(OutboxMessage message)
        {
            if (message == null) throw new ArgumentNullException($"{nameof(OutboxMessage)} was not provided to be send.");
            return Send(message.ToAddress, message.CcAddress, message.BccAddress, message.FromAddress, message.Subject, message.Body);
        }

        public bool Send(string to, string cc, string bcc, string from, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                // todo: extend message with names
                message.From.Add(new MailboxAddress(from?.Substring(0, from.IndexOf("@", StringComparison.Ordinal)), from));
                message.To.Add(new MailboxAddress(to?.Substring(0, to.IndexOf("@", StringComparison.Ordinal)), to));
                message.Cc.Add(new MailboxAddress(cc?.Substring(0, cc.IndexOf("@", StringComparison.Ordinal)), cc));
                message.Bcc.Add(new MailboxAddress(bcc?.Substring(0, bcc.IndexOf("@", StringComparison.Ordinal)), bcc));
                message.Subject = subject;
                message.Body = new BodyBuilder
                {
                    HtmlBody = body
                }.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect("iprint", 25, SecureSocketOptions.None); // todo: move to the configs
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
