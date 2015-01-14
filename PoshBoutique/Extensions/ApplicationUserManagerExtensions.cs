using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using PoshBoutique.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Extensions
{
    public static class ApplicationUserManagerExtensions
    {
        public static void SetUserTokenProvider(this ApplicationUserManager userManager, params string[] userManagerPurposes)
        {
            IDataProtectionProvider dataProtectorProvider = new DpapiDataProtectionProvider("PoshBoutique");
            IDataProtector dataProtector = dataProtectorProvider.Create(userManagerPurposes);
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtector);
        }
    }
}