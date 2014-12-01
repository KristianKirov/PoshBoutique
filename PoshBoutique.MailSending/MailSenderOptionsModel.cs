using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.MailSending
{
    public class MailSenderOptionsModel
    {
        public string DefaultSender { get; private set; }

        public string SmtpHost { get; private set; }

        public int SmtpPort { get; private set; }

        public bool HasCredentials
        {
            get
            {
                return this.Credentials != null;
            }
        }

        public NetworkCredential Credentials { get; set; }

        public MailSenderOptionsModel(string defaultSender, string smtpHost, int smtpPort)
        {
            this.DefaultSender = defaultSender;
            this.SmtpHost = smtpHost;
            this.SmtpPort = smtpPort;
        }
    }
}
