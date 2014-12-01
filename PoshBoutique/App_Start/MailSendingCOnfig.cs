using PoshBoutique.MailSending;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PoshBoutique
{
    public static class MailSendingConfig
    {
        public static void Configure()
        {
            MailSenderOptionsModel mailSettings = new MailSenderOptionsModel(
                ConfigurationManager.AppSettings["MailSending.DefaultSender"],
                ConfigurationManager.AppSettings["MailSending.Host"],
                int.Parse(ConfigurationManager.AppSettings["MailSending.Port"]));
            //MailSenderOptionsModel mailSettings = new MailSenderOptionsModel("sales@poshboutique.com", "poshboutique.tld", 25); //http://help.superhosting.bg/smtp-settings-in-script.html
            string mailSendingUserName = ConfigurationManager.AppSettings["MailSending.UserName"];
            string mailSendingPassword = ConfigurationManager.AppSettings["MailSending.Password"];
            if (!string.IsNullOrEmpty(mailSendingUserName) && !string.IsNullOrEmpty(mailSendingPassword))
            {
                mailSettings.Credentials = new System.Net.NetworkCredential(mailSendingUserName, mailSendingPassword);
            }

            MailSenderOptions.Configure(mailSettings);
        }
    }
}