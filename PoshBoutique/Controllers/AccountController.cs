using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PoshBoutique.Models;
using PoshBoutique.Providers;
using PoshBoutique.Results;
using PoshBoutique.Identity;
using PoshBoutique.Extensions;
using PoshBoutique.Facades;
using System.Configuration;
using System.Data.Entity;

namespace PoshBoutique.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        public AccountController()
            : this(Startup.UserManagerFactory(), Startup.OAuthOptions.AccessTokenFormat)
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public async Task<UserInfoViewModel> GetUserInfo()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = await this.UserManager.FindByIdAsync(userId);

            return new UserInfoViewModel()
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Email = currentUser.Email
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                UserName = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "?code=0&data=" + Uri.EscapeDataString(error) + "#/autherror");
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindByNameAsync(externalLogin.Email);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = externalLogin.Email,
                    Email = externalLogin.Email,
                    FirstName = externalLogin.FirstName,
                    LastName = externalLogin.LastName
                };

                user.Logins.Add(new IdentityUserLogin()
                {
                    LoginProvider = externalLogin.LoginProvider,
                    ProviderKey = externalLogin.ProviderKey,
                    UserId = user.Id
                });

                IdentityResult result = await UserManager.CreateAsync(user);
                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

                MailSendingFacade mailSender = new MailSendingFacade();
                mailSender.SendNewUserRegisteredMail(externalLogin.Email, externalLogin.FirstName, externalLogin.LastName);
            }
            else
            {
                bool isExistingLogin = user.Logins.Any(l => l.LoginProvider == externalLogin.LoginProvider && l.ProviderKey == externalLogin.ProviderKey);
                if (!isExistingLogin)
                {
                    user.Logins.Add(new IdentityUserLogin()
                    {
                        LoginProvider = externalLogin.LoginProvider,
                        ProviderKey = externalLogin.ProviderKey,
                        UserId = user.Id
                    });

                    await UserManager.UpdateAsync(user);
                }
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            ClaimsIdentity oAuthIdentity = await UserManager.CreateIdentityAsync(user,
                OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookieIdentity = await UserManager.CreateIdentityAsync(user,
                CookieAuthenticationDefaults.AuthenticationType);
            //AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user);
            Authentication.SignIn(/*properties, */oAuthIdentity, cookieIdentity);

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            MailSendingFacade mailSender = new MailSendingFacade();
            mailSender.SendNewUserRegisteredMail(model.Email, model.FirstName, model.LastName);

            return Ok();
        }

        [AllowAnonymous]
        [Route("SendResetPasswordMail")]
        [HttpGet]
        public async Task<IHttpActionResult> SendResetPasswordMail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return this.BadRequest("Email is required!");
            }

            ApplicationUser user = await this.UserManager.FindByNameAsync(email);
            if (user == null)
            {
                return this.BadRequest("User with email: " + email + " is not found!");
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return this.BadRequest("The user with email: " + email + "does not have local login!");
            }

            this.UserManager.SetUserTokenProvider("ForgottenPassword");
            string passwordResetToken = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string baseUrl = ConfigurationManager.AppSettings["Site.BaseUrl"];
            string resetPasswordPath = ConfigurationManager.AppSettings["Site.ResetPasswordPath"];
            string resetPasswordUrl = string.Concat(baseUrl, resetPasswordPath.TrimStart('/'), "/", HttpUtility.UrlEncode(email), "?token=", HttpUtility.UrlEncode(passwordResetToken));
            MailSendingFacade mailSender = new MailSendingFacade();
            mailSender.SendForgottenPasswordMail(baseUrl, resetPasswordUrl, email);

            return this.Ok();
        }

        [AllowAnonymous]
        [Route("resetpassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetPassword([FromBody]ResetPasswordBindingModel resetPasswordModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            ApplicationUser user = await this.UserManager.FindByNameAsync(resetPasswordModel.Email);
            if (user == null)
            {
                return this.BadRequest("User with email: " + resetPasswordModel.Email + " is not found!");
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                return this.BadRequest("The user with email: " + resetPasswordModel.Email + "does not have local login!");
            }

            this.UserManager.SetUserTokenProvider("ForgottenPassword");
            IdentityResult resetPasswordResult = await this.UserManager.ResetPasswordAsync(user.Id, resetPasswordModel.Token, resetPasswordModel.NewPassword);

            IHttpActionResult errorResult = GetErrorResult(resetPasswordResult);

            if (errorResult != null)
            {
                return errorResult;
            }

            return this.Ok();
        }

        [Route("Profile")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProfile()
        {
            string currentUserId = this.User.Identity.GetUserId();
            Profile currentUserProfile = null;
            using (ApplicationDbContext usersDbContext = new ApplicationDbContext())
            {
                currentUserProfile = await usersDbContext.UserProfiles.FirstOrDefaultAsync(p => p.UserId == currentUserId);
            }

            if (currentUserProfile == null)
            {
                return this.Ok();
            }

            return this.Ok(new UserProfileModel()
            {
                UserId = new Guid(currentUserId),
                TotalExpenses = currentUserProfile.TotalExpenses
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (this.Email != null)
                {
                    claims.Add(new Claim(ClaimTypes.Email, this.Email, null, LoginProvider));
                }

                if (this.FirstName != null)
                {
                    claims.Add(new Claim(ClaimTypes.GivenName, this.FirstName, null, LoginProvider));
                }

                if (this.LastName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Surname, this.LastName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    Email = identity.FindFirstValue(ClaimTypes.Email),
                    FirstName = identity.FindFirstValue(ClaimTypes.GivenName),
                    LastName = identity.FindFirstValue(ClaimTypes.Surname)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
