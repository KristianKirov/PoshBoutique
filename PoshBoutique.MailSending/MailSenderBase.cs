using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.MailSending
{
    public abstract class MailSenderBase : IMailSender
    {
        private MailMessage GetMailMessage(string to, string subject, string body)
        {
            MailMessage message = new MailMessage(MailSenderOptions.Settings.DefaultSender, to);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;
            message.Bcc.Add(MailSenderOptions.Settings.DefaultSender);
            message.SubjectEncoding = message.BodyEncoding = Encoding.Unicode;

            return message;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            if (!MailSenderOptions.IsEnabled)
            {
                return;
            }

            MailMessage message = this.GetMailMessage(to, subject, body);
            await this.SendMailMessage(message);
        }

        protected abstract Task SendMailMessage(MailMessage message);
    }
}
