using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PoshBoutique.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base (store)
        {
            ((UserValidator<ApplicationUser, string>)this.UserValidator).AllowOnlyAlphanumericUserNames = false;
        }
    }
}