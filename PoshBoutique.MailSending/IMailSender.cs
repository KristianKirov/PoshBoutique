using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.MailSending
{
    public interface IMailSender
    {
        Task SendEmail(string to, string subject, string body);
    }
}
