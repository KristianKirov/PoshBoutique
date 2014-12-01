using PoshBoutique.MailSending;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PoshBoutique.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send()
        {
            IMailSender mailSender = new SystemMailSender();
            mailSender.SendEmail("kristiankirov9112@gmail.com", "uraa", "<h1>Здрасти!</h1>");

            return this.Content("Done!");
        }
    }
}
