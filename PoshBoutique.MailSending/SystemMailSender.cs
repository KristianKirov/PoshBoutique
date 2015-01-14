using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PoshBoutique.MailSending
{
    public class SystemMailSender : MailSenderBase
    {
        private SmtpClient GetMailClient()
        {
            SmtpClient mailClient = new SmtpClient(MailSenderOptions.Settings.SmtpHost, MailSenderOptions.Settings.SmtpPort);

            if (MailSenderOptions.Settings.HasCredentials)
            {
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = MailSenderOptions.Settings.Credentials;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            }

            return mailClient;
        }

        protected override async Task SendMailMessage(MailMessage message)
        {
            using (SmtpClient mailClient = this.GetMailClient())
            {
                using (message)
                {
                    await mailClient.SendMailAsync(message);
                }
            }
        }
    }
}