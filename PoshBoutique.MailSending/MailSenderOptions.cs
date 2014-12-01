using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.MailSending
{
    public static class MailSenderOptions
    {
        internal static bool IsEnabled { get; private set; }

        internal static MailSenderOptionsModel Settings { get; private set; }

        static MailSenderOptions()
        {
            MailSenderOptions.IsEnabled = false;
        }

        public static void Configure(MailSenderOptionsModel settings)
        {
            MailSenderOptions.IsEnabled = true;

            MailSenderOptions.Settings = settings;
        }
    }
}
