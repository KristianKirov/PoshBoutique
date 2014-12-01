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
            message.Body = body;
            message.Bcc.Add(to);

            return message;
        }

        public void SendEmail(string to, string subject, string body)
        {
            if (!MailSenderOptions.IsEnabled)
            {
                return;
            }

            MailMessage message = this.GetMailMessage(to, subject, body);
            this.SendMailMessage(message);
        }

        protected abstract void SendMailMessage(MailMessage message);
    }
}
