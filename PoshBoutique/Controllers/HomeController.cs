using PoshBoutique.Identity;
using PoshBoutique.MailSending;
using PoshBoutique.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PoshBoutique.Extensions;
using PoshBoutique.Facades;
using System.Data.Entity;
using System.Configuration;

namespace PoshBoutique.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Send()
        {
            Guid userId = new Guid("5c771db7-ee95-4bda-8bbe-f44eef49de79");
            string userIdString = userId.ToString();

            using (ApplicationUserManager userManager = Startup.UserManagerFactory())
            {
                userManager.SetUserTokenProvider("ForgottenPassword");

                string passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(userIdString);

                //ApplicationUser user = await userManager.Users.FirstAsync(u => u.Id == userIdString);
                string baseUrl = ConfigurationManager.AppSettings["Site.BaseUrl"];
                string resetPasswordPath = ConfigurationManager.AppSettings["Site.ResetPasswordPath"];
                string resetPasswordUrl = string.Concat(baseUrl, resetPasswordPath.TrimStart('/'), "/", passwordResetToken);
                MailSendingFacade mailSender = new MailSendingFacade();
                mailSender.SendForgottenPasswordMail(baseUrl, resetPasswordUrl, userIdString);
            }

            return this.Content("Done!");
        }
    }
}
